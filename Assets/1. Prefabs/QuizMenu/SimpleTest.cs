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
    private Button restartButton;
    private TextField textField;
    private Action restartAction;
    private KanaRomajiTranslator textConverter;

    private string currentAnswer;
    private int amountCorrect;

    private List<VerbConjugation> questionTypes;

    [CreateProperty] public string QuestionType => questionType;
    private string questionType;

    [CreateProperty] public string PreviousAnswer => feedbackText;
    private string feedbackText;

    [CreateProperty] public string CurrentKanji => currentKanji;
    private string currentKanji;

    [CreateProperty] public string CurrentKana => currentKana;
    private string currentKana;

    [CreateProperty] public string CurrentQuestion => (currentQuestion + 1).ToString();
    private int currentQuestion;

    [CreateProperty] public string TotalQuestions => totalQuestions.ToString();
    private int totalQuestions;

    public List<Verb> quizQuestions;

    private List<(string, VerbConjugation)> askedQuestions = new();

    [SerializeField] private InputManager inputManager;
    private bool confirmAnswer = false;
    public bool StrictMode = false;
    void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.dataSource = this;
        textField = root.MQ<TextField>("TextField");
        submitButton = root.MQ<Button>("Submit");
        submitButton.clicked += OnPressSubmit;

        restartButton = root.MQ<Button>("Restart");
        restartButton.clicked += RestartQuiz;

        textConverter = GetComponent<KanaRomajiTranslator>();
        textConverter.InputChanged += OnInputChanged;
        textField.RegisterCallback<NavigationSubmitEvent>(evt =>
        {
            evt.StopImmediatePropagation();
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
        quizQuestions = config.words;

        askedQuestions = new();
        restartButton.SetEnabled(true);
        restartButton.visible = false;
        feedbackText = "";
        InitializeQuestionTypes(config);
        if (QuestionCount > 0)
        {
            totalQuestions = QuestionCount;
        }
        else
        {
            totalQuestions = quizQuestions.Count;
        }
        currentQuestion = 0;
        amountCorrect = 0;

        this.restartAction = restartAction;
        PrepareNextQuestion();
    }

    private VerbConjugation GetQuestionType()
    {
        int index = UnityEngine.Random.Range(0, questionTypes.Count);
        return questionTypes[index];
    }

    private void OnInputChanged()
    {
        confirmAnswer = false;
        textField.style.color = Color.black;
    }

    private void OnPressSubmit()
    {
        if (textField.value.ContainsInvalidCharacters())
        {
            feedbackText = $"Text must only contain japanese characters";
            textField.style.color = Color.maroon;
            textField.Focus();
            return;
        }

        if (shouldConfirmAnswer == false)
        {
            if (textField.value == currentAnswer)
            {
                feedbackText = "Correct!";
                confirmAnswer = true;
                textField.Focus();
                textField.style.color = Color.forestGreen;
                return;
            }
            else
            {
                feedbackText = $"Wrong! Try again?";
                confirmAnswer = true;
                textField.Focus();
                textField.style.color = Color.maroon;
                return;
            }
        }
        else
        {
            if (textField.value == currentAnswer)
            {
                feedbackText = "";
                //Play Correct VFX
                amountCorrect += 1;
            }
            else
            {
                feedbackText = $"The correct answer was {currentAnswer}.";
            }
        }

        textField.value = "";
        confirmAnswer = false;

        if (currentQuestion >= totalQuestions - 1)
        {
            EndQuiz();
            return;
        }
        currentQuestion++;
        PrepareNextQuestion();
    }

    private bool shouldConfirmAnswer => StrictMode || confirmAnswer ? true : false;

    private void PrepareNextQuestion()
    {
        if (currentQuestion >= totalQuestions) 
        {
            EndQuiz();
            return;
        }

        (int, VerbConjugation) question = GetQuestion();

        int wordIndex = question.Item1;
        VerbConjugation form = question.Item2;

        switch (form)
        {
        case VerbConjugation.PoliteNonpast:
            currentAnswer = quizQuestions[wordIndex].PoliteNonpast;
            questionType = "Polite Non-past Form";
            break;
        case VerbConjugation.PoliteNonpastNegative:
            currentAnswer = quizQuestions[wordIndex].PoliteNonPastNegative;
            questionType = "Polite Non-past Negative Form";
            break;
        case VerbConjugation.PolitePast:
            currentAnswer = quizQuestions[wordIndex].PolitePast;
            questionType = "Polite Past Form";
            break;
        case VerbConjugation.PolitePastNegative:
            currentAnswer = quizQuestions[wordIndex].PolitePastNegative;
            questionType = "Polite Past Negative Form";
            break;
        case VerbConjugation.StandardPast:
            currentAnswer = quizQuestions[wordIndex].StandardPast;
            questionType = "Standard Past Form";
            break;
        case VerbConjugation.StandardNonpast:
            currentAnswer = quizQuestions[wordIndex].StandardNonpast;
            questionType = "Standard Past Form";
            break;
        case VerbConjugation.StandardNonpastNegative:
            currentAnswer = quizQuestions[wordIndex].StandardNonpastNegative;
            questionType = "Standard Non-past Negative Form";
            break;
        case VerbConjugation.StandardPastNegative:
            currentAnswer = quizQuestions[wordIndex].StandardPastNegative;
            questionType = "Standard Past Negative Form";
            break;
        case VerbConjugation.PoliteVolitional:
            currentAnswer = quizQuestions[wordIndex].PoliteVolitional;
            questionType = "Polite Volitional Form";
            break;
        case VerbConjugation.TeForm:
            currentAnswer = quizQuestions[wordIndex].TeForm;
            questionType = "Te-form";
            break;
        case VerbConjugation.CasualVolitional:
            currentAnswer = quizQuestions[wordIndex].CasualVolitional;
            questionType = "Casual Volitional Form";
            break;
        default:
            Debug.LogWarning("Error: Question Type not valid");
            break;
        }
        currentKanji = quizQuestions[wordIndex].kanji;
        currentKana = quizQuestions[wordIndex].Kana;
        askedQuestions.Add((quizQuestions[wordIndex].Kana, form));
        textField.Focus();
    }

    private (int, VerbConjugation) GetQuestion()
    {
        VerbConjugation form = GetQuestionType();
        int wordIndex = UnityEngine.Random.Range(0, quizQuestions.Count);
        if (askedQuestions.Contains((quizQuestions[wordIndex].Kana, form)))
        {
            return GetQuestion();
        }
        return (wordIndex, form);
    }

    private void EndQuiz()
    {
        feedbackText += $" Quiz Complete! {amountCorrect} / {totalQuestions}.";
        restartButton.SetEnabled(true);
        submitButton.SetEnabled(false);
        restartButton.visible = true;
    }

    private void RestartQuiz()
    {
        if (restartAction != null)
        {
            submitButton.SetEnabled(true);
            restartAction.Invoke();
        }
    }

    private void OnDestroy()
    {
        submitButton.clicked -= OnPressSubmit;
        textConverter.InputChanged -= OnInputChanged;
    }
}
