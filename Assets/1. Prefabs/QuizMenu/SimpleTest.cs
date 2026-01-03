using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleTest : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button submitButton;
    private TextField textField;

    private string currentAnswer;

    [CreateProperty] public string CurrentKanji => currentKanji;
    private string currentKanji;

    [CreateProperty] public string CurrentKana => currentKana;
    private string currentKana;

    [CreateProperty] public string CurrentQuestion => currentQuestion.ToString();
    private int currentQuestion;

    [CreateProperty] public string TotalQuestions => totalQuestions.ToString();
    private int totalQuestions;

    [SerializeField] VerbList quizQuestions;

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
    }

    private void InitializeQuiz()
    {
        totalQuestions = quizQuestions.List.Count;
        currentQuestion = 0;
        currentAnswer = quizQuestions.List[currentQuestion].Kana;
        currentKanji = quizQuestions.List[currentQuestion].kanji;
        currentKana = quizQuestions.List[currentQuestion].Kana;
    }


    private void OnPressSubmit()
    {
        if (textField.value == currentAnswer)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log($"Wrong! the correct answer is {currentAnswer}");
        }
        textField.value = "";
        PrepareNextQuestion();
    }

    private void PrepareNextQuestion()
    {
        currentQuestion += 1;
        currentAnswer = quizQuestions.List[currentQuestion].Kana;
        currentKanji = quizQuestions.List[currentQuestion].kanji;
        currentKana = quizQuestions.List[currentQuestion].Kana;
    }
}
