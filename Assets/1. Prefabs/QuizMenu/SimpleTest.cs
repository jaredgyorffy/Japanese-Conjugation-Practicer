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
    private Action restartAction;

    private string currentAnswer;
    private int amountCorrect;

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

    private void InitializeQuestionTypes(QuizConfiguration config)
    {
        questionTypes = new List<VerbConjugation>();
        if (config.PoliteNonpastForm)
        {
            questionTypes.Add(VerbConjugation.PoliteNonpast);
        }
        if (config.PoliteNonpastNegativeForm)
        {
            questionTypes.Add(VerbConjugation.PoliteNonpastNegative);
        }
        if (config.PolitePastForm)
        {
            questionTypes.Add(VerbConjugation.PolitePast);
        }
        if (config.PolitePastNegativeForm)
        {
            questionTypes.Add(VerbConjugation.PolitePastNegative);
        }
        if (config.StandardPastForm)
        {
            questionTypes.Add(VerbConjugation.StandardPast);
        }
        if (config.StandardNonpastNegativeForm)
        {
            questionTypes.Add(VerbConjugation.StandardNonpastNegative);
        }
        if (config.StandardPastNegativeForm)
        {
            questionTypes.Add(VerbConjugation.StandardPastNegative);
        }
        if (config.PoliteVolitionalForm)
        {
            questionTypes.Add(VerbConjugation.PoliteVolitional);
        }
        if (config.CasualVolitionalForm)
        {
            questionTypes.Add(VerbConjugation.CasualVolitional);
        }
        if (config.TeForm)
        {
            questionTypes.Add(VerbConjugation.TeForm);
        }
    }

    public void InitializeQuiz(QuizConfiguration config, int QuestionCount = 0, Action restartAction = null)
    {
        previousAnswer = "";
        InitializeQuestionTypes(config);
        if (QuestionCount > 0)
        {
            totalQuestions = QuestionCount;
        }
        else
        {
            totalQuestions = quizQuestions.List.Count;
        }
        currentQuestion = 0;
        this.restartAction = restartAction;
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

        if (currentQuestion >= totalQuestions - 1)
        {
            EndQuiz();
            return;
        }
        currentQuestion++;
        PrepareNextQuestion();
    }

    private void PrepareNextQuestion()
    {
        if (currentQuestion >= totalQuestions) 
        {
            EndQuiz();
            return;
        }

        VerbConjugation form = GetQuestionType();
        //Formality > Time > Positive/Negative

        int wordIndex = UnityEngine.Random.Range(0, quizQuestions.List.Count);
        switch (form)
        {
        case VerbConjugation.PoliteNonpast:
            currentAnswer = quizQuestions.List[wordIndex].PoliteNonpast;
            questionType = "Polite Non-past Form";
            break;
        case VerbConjugation.PoliteNonpastNegative:
            currentAnswer = quizQuestions.List[wordIndex].PoliteNonPastNegative;
            questionType = "Polite Non-past Negative Form";
            break;
        case VerbConjugation.PolitePast:
            currentAnswer = quizQuestions.List[wordIndex].PolitePast;
            questionType = "Polite Past Form";
            break;
        case VerbConjugation.PolitePastNegative:
            currentAnswer = quizQuestions.List[wordIndex].PolitePastNegative;
            questionType = "Polite Past Negative Form";
            break;
        case VerbConjugation.StandardPast:
            currentAnswer = quizQuestions.List[wordIndex].StandardPast;
            questionType = "Standard Past Form";
            break;
        case VerbConjugation.StandardNonpast:
            currentAnswer = quizQuestions.List[wordIndex].StandardNonpast;
            questionType = "Standard Past Form";
            break;
        case VerbConjugation.StandardNonpastNegative:
            currentAnswer = quizQuestions.List[wordIndex].StandardNonpastNegative;
            questionType = "Standard Non-past Negative Form";
            break;
        case VerbConjugation.StandardPastNegative:
            currentAnswer = quizQuestions.List[wordIndex].StandardPastNegative;
            questionType = "Standard Past Negative Form";
            break;
        case VerbConjugation.PoliteVolitional:
            currentAnswer = quizQuestions.List[wordIndex].PoliteVolitional;
            questionType = "Polite Volitional Form";
            break;
        case VerbConjugation.TeForm:
            currentAnswer = quizQuestions.List[wordIndex].TeForm;
            questionType = "Te-form";
            break;
        case VerbConjugation.CasualVolitional:
            currentAnswer = quizQuestions.List[wordIndex].CasualVolitional;
            questionType = "Casual Volitional Form";
            break;
        default:
            Debug.LogWarning("Error: Question Type not valid");
            break;
        }
        currentKanji = quizQuestions.List[wordIndex].kanji;
        currentKana = quizQuestions.List[wordIndex].Kana;
        
        textField.Focus();
    }

    private void EndQuiz()
    {
        previousAnswer = $"Quiz Complete! {amountCorrect} / {totalQuestions}.";

        Invoke("RestartQuiz", 5);
    }

    private void RestartQuiz()
    {
        if (restartAction != null)
        {
            restartAction.Invoke();
        }
    }

    private void OnDisable()
    {
        submitButton.clicked -= OnPressSubmit;
    }
}
