using NaughtyAttributes;
using System;
using System.Collections.Generic;
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

    [SerializeField] private bool PresentTense;
    [SerializeField] private bool NegativeTense;
    public List<VerbConjugation> questionTypes;

    [CreateProperty] public string QuestionType => questionType;
    private string questionType;

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
        InitializeQuestionTypes();
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
        questionType = "Present Tense";

        textField.RegisterCallback<NavigationSubmitEvent>(evt =>
        {
            evt.StopImmediatePropagation(); // prevents internal handling
            OnPressSubmit();
        }, TrickleDown.TrickleDown);
    }

    private void InitializeQuestionTypes()
    {
        questionTypes = new List<VerbConjugation>();
        if (PresentTense)
        {
            questionTypes.Add(VerbConjugation.Present);
        }
        if (NegativeTense)
        {
            questionTypes.Add(VerbConjugation.Negative);
        }
    }

    private void InitializeQuiz()
    {
        totalQuestions = quizQuestions.List.Count;
        currentQuestion = 0;

        PrepareNextQuestion();
    }

    private VerbConjugation GetQuestionType()
    {
        int index = UnityEngine.Random.Range(0, questionTypes.Count);
        Debug.Log(index);
        return questionTypes[index];
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
        currentQuestion += 1;
        PrepareNextQuestion();
    }

    private void PrepareNextQuestion()
    {
        if (currentQuestion >= quizQuestions.List.Count) 
        {
            EndQuiz();
            return;
        }

        VerbConjugation form = GetQuestionType();

        switch (form)
        {
        case VerbConjugation.Present:
            currentAnswer = quizQuestions.List[currentQuestion].Present;
            questionType = "Present Tense";
            break;
        case VerbConjugation.Negative:
            currentAnswer = quizQuestions.List[currentQuestion].Negative;
            questionType = "Negative Tense";
            break;
        default:
            Debug.LogWarning("Error: Question Type not valid");
            break;
        }

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
