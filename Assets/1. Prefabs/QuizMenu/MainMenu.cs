using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SimpleTest test;
    [SerializeField] private VisualTreeAsset togglePrefab;
    [SerializeField] private InputManager inputManager;

    private UIDocument uiDocument;
    private VisualElement root;
    private Button startButton;

    private List<Toggle> toggles = new();

    void Start()
    {
        GetReferences();
        InitializeOptions();
    }

    private void GetReferences()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.dataSource = this;
        startButton = root.MQ<Button>("Start");
        startButton.clicked += OnPressStart;
    }

    private void OnPressStart()
    {
        test.InitializeQuiz(InitializeQuiz());
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

            toggles.Add(toggleBox.MQ<Toggle>());
            forms.Add(toggleBox);
        }
    }

    private QuizConfiguration InitializeQuiz()
    {
        QuizConfiguration config = new QuizConfiguration();
        var listOfForms = config.GetForms();
        for (int i = 0; i < toggles.Count; i++)
        {
            listOfForms[i] = (listOfForms[i].name, toggles[i].value);
        }
        config.SetForms(listOfForms);
        return config;
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