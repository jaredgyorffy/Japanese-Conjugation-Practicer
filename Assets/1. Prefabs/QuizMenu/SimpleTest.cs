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
    [SerializeField] private bool PresentNegativeTense;
    [SerializeField] private bool PastTense;
    [SerializeField] private bool PastNegativeTense;
    [SerializeField] private bool ShortPastTense;
    [SerializeField] private bool ShortPresentNegativeTense;
    [SerializeField] private bool ShortPastNegativeTense;
    [SerializeField] private bool VolitionalTense;
    [SerializeField] private bool TeFormStem;
    [SerializeField] private bool RequestTense;
    private List<VerbConjugation> questionTypes;

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
        if (PresentNegativeTense)
        {
            questionTypes.Add(VerbConjugation.PresentNegative);
        }
        if (PastTense)
        {
            questionTypes.Add(VerbConjugation.Past);
        }
        if (PastNegativeTense)
        {
            questionTypes.Add(VerbConjugation.PastNegative);
        }
        if (ShortPastTense)
        {
            questionTypes.Add(VerbConjugation.ShortPast);
        }
        if (ShortPresentNegativeTense)
        {
            questionTypes.Add(VerbConjugation.ShortPresentNegative);
        }
        if (ShortPastNegativeTense)
        {
            questionTypes.Add(VerbConjugation.ShortPastNegative);
        }
        if (VolitionalTense)
        {
            questionTypes.Add(VerbConjugation.Volitional);
        }
        if (TeFormStem)
        {
            questionTypes.Add(VerbConjugation.TeForm);
        }
        if (RequestTense)
        {
            questionTypes.Add(VerbConjugation.Request);
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
        case VerbConjugation.PresentNegative:
            currentAnswer = quizQuestions.List[currentQuestion].PresentNegative;
            questionType = "Negative Tense";
            break;
        case VerbConjugation.Past:
            currentAnswer = quizQuestions.List[currentQuestion].Past;
            questionType = "Past Tense";
            break;
        case VerbConjugation.PastNegative:
            currentAnswer = quizQuestions.List[currentQuestion].PastNegative;
            questionType = "Past Negative Tense";
            break;
        case VerbConjugation.ShortPast:
            currentAnswer = quizQuestions.List[currentQuestion].ShortPast;
            questionType = "Short Past Tense";
            break;
        case VerbConjugation.ShortPresentNegative:
            currentAnswer = quizQuestions.List[currentQuestion].ShortPresentNegative;
            questionType = "Short Present Negative Tense";
            break;
        case VerbConjugation.ShortPastNegative:
            currentAnswer = quizQuestions.List[currentQuestion].ShortPastNegative;
            questionType = "Short Past Negative Tense";
            break;
        case VerbConjugation.Volitional:
            currentAnswer = quizQuestions.List[currentQuestion].Volitional;
            questionType = "Volitional Tense";
            break;
        case VerbConjugation.TeForm:
            currentAnswer = quizQuestions.List[currentQuestion].TeForm;
            questionType = "Te-form Stem";
            break;
        case VerbConjugation.Request:
            currentAnswer = quizQuestions.List[currentQuestion].Request;
            questionType = "Polite Request";
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
