using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button startButton;
    [SerializeField] private SimpleTest test;
    [SerializeField] private VisualTreeAsset togglePrefab;

    private List<Toggle> toggles = new();

    [SerializeField] private InputManager inputManager;
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
    public bool NonpastPoliteForm;
    public bool NonpastNegativePoliteForm;
    public bool PastPoliteForm;
    public bool PastNegativePoliteForm;
    public bool PastStandardForm;
    public bool NonpastNegativeStandardForm;
    public bool PastNegativeStandardForm;
    public bool VolitionalForm;
    public bool TeFormStem;
    public bool TeForm;
    public bool PoliteVolitionalForm;
    public bool StandardVolitionalForm;

    public List<(string name, bool on)> GetForms()
    {
        return new List<(string name, bool on)>
        {
            ("Non-past Polite Form", false),
            ("Non-past Negative Polite Form", false),
            ("Past Polite Form", false),
            ("Past Negative Polite Form", false),
            ("Past Standard Form", false),
            ("Nonpast Negative Standard Form", false),
            ("Past Negative Standard Form", false),
            ("Volitional Form", false),
            ("Te-Form Stem", false),
            ("Te-Form", false),
            ("Polite Volitional Form", false),
            ("Standard Volitional Form", false)
        };
    }

    public void SetForms(List<(string name, bool on)> formsEnabled)
    {
        if (formsEnabled.Count < 12)
            throw new ArgumentException("formsEnabled must have at least 12 elements.");

        NonpastPoliteForm = formsEnabled[0].on;
        NonpastNegativePoliteForm = formsEnabled[1].on;
        PastPoliteForm = formsEnabled[2].on;
        PastNegativePoliteForm = formsEnabled[3].on;
        PastStandardForm = formsEnabled[4].on;
        NonpastNegativeStandardForm = formsEnabled[5].on;
        PastNegativeStandardForm = formsEnabled[6].on;
        VolitionalForm = formsEnabled[7].on;
        TeFormStem = formsEnabled[8].on;
        TeForm = formsEnabled[9].on;
        PoliteVolitionalForm = formsEnabled[10].on;
        StandardVolitionalForm = formsEnabled[11].on;
    }
}