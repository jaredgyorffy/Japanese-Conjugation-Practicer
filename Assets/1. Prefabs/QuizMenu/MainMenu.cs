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

    [SerializeField] private InputManager inputManager;
    void Start()
    {
        GetReferences();
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
        test.InitializeQuiz(GetSettings());
        root.visible = false;
    }

    private QuizConfiguration GetSettings()
    {
        QuizConfiguration config = new QuizConfiguration();
        
        return config;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

public class QuizConfiguration
{
    public bool PresentTense = true;
    public bool PresentNegativeTense;
    public bool PastTense;
    public bool PastNegativeTense;
    public bool ShortPastTense;
    public bool ShortPresentNegativeTense;
    public bool ShortPastNegativeTense;
    public bool VolitionalTense;
    public bool TeFormStem;
    public bool RequestTense;
}