using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SimpleTest : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button submitButton;
    private TextField textField;

    private string currentAnswer;
    private int amountCorrect;

    [CreateProperty] public string PreviousAnswer => previousAnswer;
    private string previousAnswer;

    [CreateProperty] public string CurrentKanji => currentKanji;
    private string currentKanji;

    [CreateProperty] public string CurrentKana => currentKana;
    private string currentKana;

    [CreateProperty] public string CurrentQuestion => (currentQuestion + 1).ToString();
    private int currentQuestion;

    [CreateProperty] public string TotalQuestions => totalQuestions.ToString();
    private int totalQuestions;

    [SerializeField] VerbList quizQuestions;

    [SerializeField] private InputManager inputManager;
    void Start()
    {
        GetReferences();

        InitializeQuiz();
    }

    private void GetReferences()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.dataSource = this;
        submitButton = root.MQ<Button>("Submit");
        textField = root.MQ<TextField>("TextField");
        submitButton.clicked += OnPressSubmit;

        textField.RegisterCallback<NavigationSubmitEvent>(evt =>
        {
            evt.StopImmediatePropagation(); // prevents internal handling
            OnPressSubmit();
        }, TrickleDown.TrickleDown);
    }

    private void InitializeQuiz()
    {
        totalQuestions = quizQuestions.List.Count;
        currentQuestion = 0;
        currentAnswer = quizQuestions.List[currentQuestion].Kana;
        currentKanji = quizQuestions.List[currentQuestion].kanji;
        currentKana = quizQuestions.List[currentQuestion].Kana;
        textField.MQ(TextInputBaseField<string>.textInputUssName).Focus();
    }

    private void OnPressSubmit()
    {
        if (textField.value == currentAnswer)
        {
            previousAnswer = "Correct!";
            amountCorrect += 1;
        }
        else
        {
            previousAnswer = $"Wrong! the correct answer is {currentAnswer}";
        }
        textField.value = "";
        PrepareNextQuestion();
    }

    private void PrepareNextQuestion()
    {
        if (currentQuestion == quizQuestions.List.Count - 1) 
        {
            EndQuiz();
            return;
        }
        currentQuestion += 1;
        currentAnswer = quizQuestions.List[currentQuestion].Kana;
        currentKanji = quizQuestions.List[currentQuestion].kanji;
        currentKana = quizQuestions.List[currentQuestion].Kana;

        textField.Focus();
    }

    private void EndQuiz()
    {
        previousAnswer = $"Quiz Complete! {amountCorrect} / {totalQuestions}.";
    }

    private void OnDisable()
    {
        submitButton.clicked -= OnPressSubmit;
    }
}
