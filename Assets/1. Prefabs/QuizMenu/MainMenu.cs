using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SequenceTest sequenceTest;
    [SerializeField] private AdventureMode adventure;
    [SerializeField] private VisualTreeAsset togglePrefab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GlobalVariables globalVariables;

    private UIDocument uiDocument;
    private VisualElement root;
    private Button startButton;
    private Button adventureButton;
    private IntegerField questionCount;
    private IntegerField AdventureLength;
    private IntegerField StartingHealth;
    private Label warningText;

    private Toggle adventureMode;
    private List<Toggle> meaningToggles = new();
    private List<Toggle> verbFormToggles = new();
    private List<Toggle> adjectiveFormToggles = new();
    private List<Toggle> nounFormToggles = new();
    private List<Toggle> contentToggles = new();
    private List<Toggle> optionsToggles = new();

    void Start()
    {
        GetReferences();
        InitializeOptions();
    }

    private void GetReferences()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement.MQ<VisualElement>("StartScreen");
        root.dataSource = this;
        startButton = root.MQ<Button>("Start");
        questionCount = root.MQ<IntegerField>("QuestionCount");
        AdventureLength = root.MQ<IntegerField>("AdventureLength");
        StartingHealth = root.MQ<IntegerField>("StartingHealth");
        warningText = root.MQ<Label>("Warning");
        adventureButton = root.MQ<Button>("Adventure");
        startButton.clicked += OnPressStart;
        adventureButton.clicked += Adventure;
        warningText.visible = false;
    }

    private void Adventure()
    {
        QuizConfiguration config = InitializeQuiz();
        if (sequenceTest.Initialized)
        {
            sequenceTest.Unsubscribe();
        }

        int adventureLength = 3;
        if (AdventureLength != null)
        {
            adventureLength = AdventureLength.value;
        }


        int startingHealth = 3;
        if (AdventureLength != null)
        {
            startingHealth = StartingHealth.value;
        }
        if (startingHealth <= 0 || adventureLength <= 0)
        {
            warningText.visible = true;
            warningText.text = "Invalid Adventure Configuration";
            return;
        }
        adventure.InitializeAdventure(startingHealth, adventureLength, config, Restart);
        root.visible = false;
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

        if (config.Verbs.Count == 0 && config.Adjectives.Count == 0 && config.Nouns.Count == 0)
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

        if ((config.VerbQuestionTypes() * config.Verbs.Count) + (config.AdjectiveQuestionTypes() * config.Adjectives.Count + (config.NounQuestionTypes() * config.Nouns.Count)) < questions)
        {
            warningText.visible = true;
            warningText.text = "Not enough questions available with selected parameters";
            return;
        }

        warningText.visible = false;

        if (adventure.Initialized)
        {
            adventure.Unsubscribe();
        }
        sequenceTest.InitializeQuiz(config, questions, Restart);
        root.visible = false;
    }

    private void InitializeOptions()
    {
        QuizConfiguration config = new QuizConfiguration();

        VisualElement gamemode = root.MQ<VisualElement>("Gamemode");
        var toggle = togglePrefab.Instantiate();
        toggle.MQ<Label>().text = "AdventureMode";
        adventureMode = toggle.MQ<Toggle>();
        gamemode.Add(toggle);


        VisualElement meanings = root.MQ<VisualElement>("Meanings");
        var listOfMeanings = config.GetMeanings();
        for (int i = 0; i < listOfMeanings.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            toggleBox.MQ<Label>().text = listOfMeanings[i].name;

            meaningToggles.Add(toggleBox.MQ<Toggle>());
            meanings.Add(toggleBox);
        }

        VisualElement verbForms = root.MQ<VisualElement>("VerbForms");
        var listOfVerbForms = config.GetVerbForms();
        for (int i = 0; i < listOfVerbForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            toggleBox.MQ<Label>().text = listOfVerbForms[i].name;

            verbFormToggles.Add(toggleBox.MQ<Toggle>());
            verbForms.Add(toggleBox);
        }

        VisualElement adjectiveForms = root.MQ<VisualElement>("AdjectiveForms");
        var listOfAdjectiveForms = config.GetAdjectiveForms();
        for (int i = 0; i < listOfAdjectiveForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            toggleBox.MQ<Label>().text = listOfAdjectiveForms[i].name;

            adjectiveFormToggles.Add(toggleBox.MQ<Toggle>());
            adjectiveForms.Add(toggleBox);
        }

        VisualElement nounForms = root.MQ<VisualElement>("NounForms");
        var listOfNounForms = config.GetNounForms();
        for (int i = 0; i < listOfNounForms.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            toggleBox.MQ<Label>().text = listOfNounForms[i].name;

            nounFormToggles.Add(toggleBox.MQ<Toggle>());
            nounForms.Add(toggleBox);
        }


        VisualElement content = root.MQ<VisualElement>("Content");
        for (int i = 0; i < globalVariables.WordLists.Count; i++)
        {
            var toggleBox = togglePrefab.Instantiate();
            toggleBox.MQ<Label>().text = globalVariables.WordLists[i].listName;

            if (globalVariables.WordLists[i].listName.Contains("Debug") == false)
            {
                toggleBox.MQ<Toggle>().value = true;
            }

            contentToggles.Add(toggleBox.MQ<Toggle>());
            content.Add(toggleBox);
        }
        
        VisualElement option = root.MQ<VisualElement>("Options");
        
        var strictModeOption = togglePrefab.Instantiate();
        strictModeOption.MQ<Label>().text = "Strict mode";
        
        optionsToggles.Add(strictModeOption.MQ<Toggle>());
        option.Add(strictModeOption);
    }

    private QuizConfiguration InitializeQuiz()
    {
        QuizConfiguration config = new QuizConfiguration();

        config.AdventureMode = adventureMode.value;

        var listOfMeanings = config.GetMeanings();
        for (int i = 0; i < meaningToggles.Count; i++)
        {
            listOfMeanings[i] = (listOfMeanings[i].name, meaningToggles[i].value);
        }

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

        var listOfNounForms = config.GetNounForms();
        for (int i = 0; i < nounFormToggles.Count; i++)
        {
            listOfNounForms[i] = (listOfNounForms[i].name, nounFormToggles[i].value);
        }

        config.SetMeanings(listOfMeanings);
        config.SetVerbForms(listOfVerbForms);
        config.SetAdjectiveForms(listOfAdjectiveForms);
        config.SetNounForms(listOfNounForms);

        for (int i = 0; i < contentToggles.Count; i++)
        {
            if (contentToggles[i] != null && contentToggles[i].value)
            {
                config.Verbs.AddRange(globalVariables.WordLists[i].verbList);
                config.Adjectives.AddRange(globalVariables.WordLists[i].adjectiveList);
                config.Nouns.AddRange(globalVariables.WordLists[i].nounList);
            }
        }

        config.Strictmode = optionsToggles[0].value;

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
    public bool AdventureMode;

    public bool VerbMeaning;
    public bool VerbPoliteNonpastForm;
    public bool VerbPoliteNonpastNegativeForm;
    public bool VerbPolitePastForm;
    public bool VerbPolitePastNegativeForm;
    public bool VerbStandardPastForm;
    public bool VerbStandardNonpastNegativeForm;
    public bool VerbStandardPastNegativeForm;
    public bool VerbPoliteVolitionalForm;
    public bool VerbTeForm;

    public bool AdjectiveMeaning;
    public bool AdjectivePoliteNonpastNegativeForm;
    public bool AdjectivePolitePastForm;
    public bool AdjectivePolitePastNegativeForm;
    public bool AdjectiveStandardPastForm;
    public bool AdjectiveStandardNonpastNegativeForm;
    public bool AdjectiveStandardPastNegativeForm;

    public bool NounMeaning;
    public bool NounPoliteNonpastNegativeForm;
    public bool NounPolitePastForm;
    public bool NounPolitePastNegativeForm;
    public bool NounStandardPastForm;
    public bool NounStandardNonpastNegativeForm;
    public bool NounStandardPastNegativeForm;

    public bool Strictmode = false;


    public List<Verb> Verbs = new();
    public List<Adjective> Adjectives = new();
    public List<Noun> Nouns = new();
    public bool IsValid()
    {
        return AdjectiveSelected() || VerbsSelected() || NounSelected();
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
        || VerbTeForm
        || VerbMeaning;
    }

    public bool AdjectiveSelected()
    {
        return AdjectivePoliteNonpastNegativeForm
        || AdjectivePolitePastForm
        || AdjectivePolitePastNegativeForm
        || AdjectiveStandardPastForm
        || AdjectiveStandardNonpastNegativeForm
        || AdjectiveStandardPastNegativeForm
        || AdjectiveMeaning;
    }

    public bool NounSelected()
    {
        return NounPoliteNonpastNegativeForm
        || NounPolitePastForm
        || NounPolitePastNegativeForm
        || NounStandardPastForm
        || NounStandardNonpastNegativeForm
        || NounStandardPastNegativeForm
        || NounMeaning;
    }

    public bool MeaningSelected()
    {
        return NounMeaning
        || VerbMeaning
        || AdjectiveMeaning;
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
        (VerbTeForm ? 1 : 0) +
        (VerbMeaning ? 1 :0);
    }

    public int AdjectiveQuestionTypes()
    {
        return
        (AdjectivePoliteNonpastNegativeForm ? 1 : 0) +
        (AdjectivePolitePastForm ? 1 : 0) +
        (AdjectivePolitePastNegativeForm ? 1 : 0) +
        (AdjectiveStandardPastForm ? 1 : 0) +
        (AdjectiveStandardNonpastNegativeForm ? 1 : 0) +
        (AdjectiveStandardPastNegativeForm ? 1 : 0) +
        (AdjectiveMeaning ? 1 : 0);
    }

    public int NounQuestionTypes()
    {
        return
        (NounPoliteNonpastNegativeForm ? 1 : 0) +
        (NounPolitePastForm ? 1 : 0) +
        (NounPolitePastNegativeForm ? 1 : 0) +
        (NounStandardPastForm ? 1 : 0) +
        (NounStandardNonpastNegativeForm ? 1 : 0) +
        (NounStandardPastNegativeForm ? 1 : 0) +
        (NounMeaning ? 1 : 0);
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
            ("Te-Form", false),
        };
    }

    public List<(string name, bool on)> GetMeanings()
    {
        return new List<(string name, bool on)>
        {
            ("Noun Meanings", false),
            ("Verb Meanings", false),
            ("AdjectiveMeanings", false),
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

    public List<(string name, bool on)> GetNounForms()
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

    public void SetMeanings(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 3)
            throw new ArgumentException("formsEnabled must have at least 3 elements.");

        VerbMeaning = formsEnabled[0].on;
        AdjectiveMeaning = formsEnabled[1].on;
        NounMeaning = formsEnabled[2].on;
    }

    public void SetVerbForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 9)
            throw new ArgumentException("formsEnabled must have at least 11 elements.");

        VerbPoliteNonpastForm = formsEnabled[0].on;
        VerbPoliteNonpastNegativeForm = formsEnabled[1].on;
        VerbPolitePastForm = formsEnabled[2].on;
        VerbPolitePastNegativeForm = formsEnabled[3].on;
        VerbStandardPastForm = formsEnabled[4].on;
        VerbStandardNonpastNegativeForm = formsEnabled[5].on;
        VerbStandardPastNegativeForm = formsEnabled[6].on;
        VerbPoliteVolitionalForm = formsEnabled[7].on;
        VerbTeForm = formsEnabled[8].on;
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

    public void SetNounForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 6)
            throw new ArgumentException("Noun Forms must have at least 6 elements.");

        NounPoliteNonpastNegativeForm = formsEnabled[0].on;
        NounPolitePastForm = formsEnabled[1].on;
        NounPolitePastNegativeForm = formsEnabled[2].on;
        NounStandardPastForm = formsEnabled[3].on;
        NounStandardNonpastNegativeForm = formsEnabled[4].on;
        NounStandardPastNegativeForm = formsEnabled[5].on;
    }
}