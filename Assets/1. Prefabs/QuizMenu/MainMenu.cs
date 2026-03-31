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

    private List<Toggle> formToggles = new();
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

        if (config.words.Count == 0)
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

        if ((config.QuestionTypes() * config.words.Count) < questions)
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
        VisualElement forms = root.MQ<VisualElement>("Forms");
        var listOfForms = config.GetForms();
        for (int i = 0; i < listOfForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            var toggleLayer = toggleBox.MQ<Label>().text = listOfForms[i].name;

            formToggles.Add(toggleBox.MQ<Toggle>());
            forms.Add(toggleBox);
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
        var listOfForms = config.GetForms();
        for (int i = 0; i < formToggles.Count; i++)
        {
            listOfForms[i] = (listOfForms[i].name, formToggles[i].value);
        }

        for (int i = 0; i < contentToggles.Count; i++)
        {
            if (contentToggles[i] != null && contentToggles[i].value)
            {
                config.words.AddRange(globalVariables.WordLists[i].List);
            }
        }

        config.SetForms(listOfForms);
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
    public bool PoliteNonpastForm;
    public bool PoliteNonpastNegativeForm;
    public bool PolitePastForm;
    public bool PolitePastNegativeForm;
    public bool StandardPastForm;
    public bool StandardNonpastNegativeForm;
    public bool StandardPastNegativeForm;
    public bool PoliteVolitionalForm;
    public bool CasualVolitionalForm;
    public bool TeForm;

    public List<Verb> words = new();

    public bool IsValid()
    {
        return PoliteNonpastForm
        || PoliteNonpastNegativeForm
        || PolitePastForm
        || PolitePastNegativeForm
        || StandardPastForm
        || StandardNonpastNegativeForm
        || StandardPastNegativeForm
        || PoliteVolitionalForm
        || CasualVolitionalForm
        || TeForm;
    }

    public int QuestionTypes()
    {
        return
        (PoliteNonpastForm ? 1 : 0) +
        (PoliteNonpastNegativeForm ? 1 : 0) +
        (PolitePastForm ? 1 : 0) +
        (PolitePastNegativeForm ? 1 : 0) +
        (StandardPastForm ? 1 : 0) +
        (StandardNonpastNegativeForm ? 1 : 0) +
        (StandardPastNegativeForm ? 1 : 0) +
        (PoliteVolitionalForm ? 1 : 0) +
        (CasualVolitionalForm ? 1 : 0) +
        (TeForm ? 1 : 0);
    }

    public List<(string name, bool on)> GetForms()
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

    public void SetForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 10)
            throw new ArgumentException("formsEnabled must have at least 11 elements.");

        PoliteNonpastForm = formsEnabled[0].on;
        PoliteNonpastNegativeForm = formsEnabled[1].on;
        PolitePastForm = formsEnabled[2].on;
        PolitePastNegativeForm = formsEnabled[3].on;
        StandardPastForm = formsEnabled[4].on;
        StandardNonpastNegativeForm = formsEnabled[5].on;
        StandardPastNegativeForm = formsEnabled[6].on;
        PoliteVolitionalForm = formsEnabled[7].on;
        CasualVolitionalForm = formsEnabled[8].on;
        TeForm = formsEnabled[9].on;
    }
}