using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SimpleTest test;
    [SerializeField] private VisualTreeAsset togglePrefab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GlobalVariables globalVariables;

    private UIDocument uiDocument;
    private VisualElement root;
    private Button startButton;
    private IntegerField questionCount;
    private Label warningText;

    private List<Toggle> verbFormToggles = new();
    private List<Toggle> adjectiveFormToggles = new();
    private List<Toggle> contentToggles = new();

    void Start()
    {
        GetReferences();
        InitializeOptions();
    }

    private void GetReferences()
    {
        //using UnityEngine.AddressableAssets;
        //var asyncHandle = Addressables.LoadAssetAsync<GlobalTuner>("GlobalTuner.asset");
        //GlobalTuner globalTuner = asyncHandle.WaitForCompletion();

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.dataSource = this;
        startButton = root.MQ<Button>("Start");
        questionCount = root.MQ<IntegerField>("QuestionCount");
        warningText = root.MQ<Label>("Warning");
        startButton.clicked += OnPressStart;
        warningText.visible = false;
    }

    private void OnPressStart()
    {
        QuizConfiguration config = InitializeQuiz();
        if (config.IsValid() == false)
        {
            warningText.visible = true;
            warningText.text = "No Question Types Selected";
            return;
        }

        if (config.Verbs.Count == 0 && config.Adjectives.Count == 0)
        {
            warningText.visible = true;
            warningText.text = "No Content Types Selected";
            return;
        }

        int questions = 0;
        if (questionCount != null)
        {
            questions = questionCount.value;
        }

        if ((config.VerbQuestionTypes() * config.Verbs.Count) + (config.AdjectiveQuestionTypes() * config.Adjectives.Count) < questions)
        {
            warningText.visible = true;
            warningText.text = "Not enough questions available with selected parameters";
            return;
        }

        warningText.visible = false;
        test.InitializeQuiz(config, questions, Restart);
        root.visible = false;
    }

    private void InitializeOptions()
    {
        QuizConfiguration config = new QuizConfiguration();
        VisualElement verbForms = root.MQ<VisualElement>("VerbForms");
        var listOfVerbForms = config.GetVerbForms();
        for (int i = 0; i < listOfVerbForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            var toggleLayer = toggleBox.MQ<Label>().text = listOfVerbForms[i].name;

            verbFormToggles.Add(toggleBox.MQ<Toggle>());
            verbForms.Add(toggleBox);
        }

        VisualElement adjectiveForms = root.MQ<VisualElement>("AdjectiveForms");
        var listOfAdjectiveForms = config.GetAdjectiveForms();
        for (int i = 0; i < listOfAdjectiveForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            var toggleLayer = toggleBox.MQ<Label>().text = listOfAdjectiveForms[i].name;

            adjectiveFormToggles.Add(toggleBox.MQ<Toggle>());
            adjectiveForms.Add(toggleBox);
        }


        VisualElement content = root.MQ<VisualElement>("Content");
        for (int i = 0; i < globalVariables.WordLists.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            var toggleLayer = toggleBox.MQ<Label>().text = globalVariables.WordLists[i].listName;

            contentToggles.Add(toggleBox.MQ<Toggle>());
            content.Add(toggleBox);
        }
    }

    private QuizConfiguration InitializeQuiz()
    {
        QuizConfiguration config = new QuizConfiguration();

        var listOfVerbForms = config.GetVerbForms();
        for (int i = 0; i < verbFormToggles.Count; i++)
        {
            listOfVerbForms[i] = (listOfVerbForms[i].name, verbFormToggles[i].value);
        }

        var listOfAdjectiveForms = config.GetAdjectiveForms();
        for (int i = 0; i < adjectiveFormToggles.Count; i++)
        {
            listOfAdjectiveForms[i] = (listOfAdjectiveForms[i].name, adjectiveFormToggles[i].value);
        }

        config.SetVerbForms(listOfVerbForms);
        config.SetAdjectiveForms(listOfAdjectiveForms);

        for (int i = 0; i < contentToggles.Count; i++)
        {
            if (contentToggles[i] != null && contentToggles[i].value)
            {
                if (config.VerbsSelected())
                {
                    config.Verbs.AddRange(globalVariables.WordLists[i].verbList);
                }
                if (config.AdjectiveSelected())
                {
                    config.Adjectives.AddRange(globalVariables.WordLists[i].adjectiveList);
                }
            }
        }

        return config;
    }

    private void Restart()
    {
        warningText.visible = false;
        root.visible = true;
    }
}

public class QuizConfiguration
{
    public bool VerbPoliteNonpastForm;
    public bool VerbPoliteNonpastNegativeForm;
    public bool VerbPolitePastForm;
    public bool VerbPolitePastNegativeForm;
    public bool VerbStandardPastForm;
    public bool VerbStandardNonpastNegativeForm;
    public bool VerbStandardPastNegativeForm;
    public bool VerbPoliteVolitionalForm;
    public bool VerbCasualVolitionalForm;
    public bool VerbTeForm;

    public bool AdjectivePoliteNonpastNegativeForm;
    public bool AdjectivePolitePastForm;
    public bool AdjectivePolitePastNegativeForm;
    public bool AdjectiveStandardPastForm;
    public bool AdjectiveStandardNonpastNegativeForm;
    public bool AdjectiveStandardPastNegativeForm;


    public List<Verb> Verbs = new();
    public List<Adjective> Adjectives = new();
    public bool IsValid()
    {
        return AdjectiveSelected() || VerbsSelected();
    }

    public bool VerbsSelected()
    {
        return VerbPoliteNonpastForm
        || VerbPoliteNonpastNegativeForm
        || VerbPolitePastForm
        || VerbPolitePastNegativeForm
        || VerbStandardPastForm
        || VerbStandardNonpastNegativeForm
        || VerbStandardPastNegativeForm
        || VerbPoliteVolitionalForm
        || VerbCasualVolitionalForm
        || VerbTeForm;
    }

    public bool AdjectiveSelected()
    {
        return AdjectivePoliteNonpastNegativeForm
        || AdjectivePolitePastForm
        || AdjectivePolitePastNegativeForm
        || AdjectiveStandardPastForm
        || AdjectiveStandardNonpastNegativeForm
        || AdjectiveStandardPastNegativeForm;
    }

    public int VerbQuestionTypes()
    {
        return
        (VerbPoliteNonpastForm ? 1 : 0) +
        (VerbPoliteNonpastNegativeForm ? 1 : 0) +
        (VerbPolitePastForm ? 1 : 0) +
        (VerbPolitePastNegativeForm ? 1 : 0) +
        (VerbStandardPastForm ? 1 : 0) +
        (VerbStandardNonpastNegativeForm ? 1 : 0) +
        (VerbStandardPastNegativeForm ? 1 : 0) +
        (VerbPoliteVolitionalForm ? 1 : 0) +
        (VerbCasualVolitionalForm ? 1 : 0) +
        (VerbTeForm ? 1 : 0);
    }

    public int AdjectiveQuestionTypes()
    {
        return
        (AdjectivePoliteNonpastNegativeForm ? 1 : 0) +
        (AdjectivePolitePastForm ? 1 : 0) +
        (AdjectivePolitePastNegativeForm ? 1 : 0) +
        (AdjectiveStandardPastForm ? 1 : 0) +
        (AdjectiveStandardNonpastNegativeForm ? 1 : 0) +
        (AdjectiveStandardPastNegativeForm ? 1 : 0);
    }

    public List<(string name, bool on)> GetVerbForms()
    {
        return new List<(string name, bool on)>
        {
            ("Polite Non-past Form", false),
            ("Polite Non-past Negative Form", false),
            ("Polite Past Form", false),
            ("Polite Past Negative Form", false),
            ("Standard Past Form", false),
            ("Standard Nonpast Negative Form", false),
            ("Standard Past Negative Form", false),
            ("Polite Volitional Form", false),
            ("Casual Volitional Form", false),
            ("Te-Form", false),
        };
    }

    public List<(string name, bool on)> GetAdjectiveForms()
    {
        return new List<(string name, bool on)>
        {
            ("Polite Non-past Negative Form", false),
            ("Polite Past Form", false),
            ("Polite Past Negative Form", false),
            ("Standard Past Form", false),
            ("Standard Nonpast Negative Form", false),
            ("Standard Past Negative Form", false),
        };
    }

    public void SetVerbForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 10)
            throw new ArgumentException("formsEnabled must have at least 11 elements.");

        VerbPoliteNonpastForm = formsEnabled[0].on;
        VerbPoliteNonpastNegativeForm = formsEnabled[1].on;
        VerbPolitePastForm = formsEnabled[2].on;
        VerbPolitePastNegativeForm = formsEnabled[3].on;
        VerbStandardPastForm = formsEnabled[4].on;
        VerbStandardNonpastNegativeForm = formsEnabled[5].on;
        VerbStandardPastNegativeForm = formsEnabled[6].on;
        VerbPoliteVolitionalForm = formsEnabled[7].on;
        VerbCasualVolitionalForm = formsEnabled[8].on;
        VerbTeForm = formsEnabled[9].on;
    }

    public void SetAdjectiveForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 6)
            throw new ArgumentException("Adjective Forms must have at least 6 elements.");

        AdjectivePoliteNonpastNegativeForm = formsEnabled[0].on;
        AdjectivePolitePastForm = formsEnabled[1].on;
        AdjectivePolitePastNegativeForm = formsEnabled[2].on;
        AdjectiveStandardPastForm = formsEnabled[3].on;
        AdjectiveStandardNonpastNegativeForm = formsEnabled[4].on;
        AdjectiveStandardPastNegativeForm = formsEnabled[5].on;
    }
}