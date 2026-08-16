using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class SequenceTest : MonoBehaviour
{
    [SerializeField] private UIDocument quizMenu;
    private VisualElement quizMenuRoot;
    private Button submitButton;
    private Button restartButton;
    private Button hintButton;
    private TextField textField;
    private Action restartAction;
    private KanaRomajiTranslator textConverter;

    public event Action<bool> AnswerSubmitted;
    public event Action NextQuestion;

    private List<string> currentAnswer = new();
    private int amountCorrect;

    private ConjugationTypes conjugationTypes;

    [CreateProperty] public string QuestionType => questionType;
    private string questionType;

    private IWord currentWord;
    private ConjugationType currentConjugationType;
    private WordType currentWordType;

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

    public WordLists WordLists;

    private List<(string, ConjugationType)> askedQuestions = new();
    
    private bool confirmAnswer = false;
    private bool hintVisible = false;
        
    private bool StrictMode = false;

    public bool Initialized { get; private set; }

    private void TryInitialize()
    {
        if (Initialized) return;
        quizMenuRoot = quizMenu.rootVisualElement;
        quizMenuRoot.dataSource = this;
        textField = quizMenuRoot.MQ<TextField>("TextField");
        submitButton = quizMenuRoot.MQ<Button>("Submit");
        submitButton.clicked += OnPressSubmit;

        restartButton = quizMenuRoot.MQ<Button>("Restart");
        restartButton.clicked += RestartQuiz;
        

        hintButton = quizMenuRoot.MQ<Button>("Hint");
        hintButton.clicked += ToggleHint;
        
        textConverter = quizMenu.GetComponent<KanaRomajiTranslator>();
        textConverter.InputChanged += OnInputChanged;

        Initialized = true;
        /*textField.RegisterCallback<BlurEvent>(evt =>
        {
            if (MobileKeyboardInput.CheckInput() == TouchScreenKeyboard.Status.Done)
            {
                evt.StopImmediatePropagation();
                OnPressSubmit();
            }
        });*/

        textField.RegisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);
    }

    public void Unsubscribe()
    {
        submitButton.clicked -= OnPressSubmit;
        restartButton.clicked -= RestartQuiz;
        hintButton.clicked -= ToggleHint;
        textConverter.InputChanged -= OnInputChanged;
        Initialized = false;
        textField.UnregisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);
    }

    private void OnPressEnterToSubmit(NavigationSubmitEvent evt)
    {
        evt.StopImmediatePropagation();
        OnPressSubmit();
    }

    private void InitializeQuestionTypes(QuizConfiguration config)
    {
        ConjugationTypes conjugation = QuizUtility.InitializeQuestionTypes(config);
        conjugationTypes = conjugation;
    }

    public void InitializeQuiz(QuizConfiguration config, int QuestionCount = 0, Action restartAction = null)
    {
        TryInitialize();
        WordLists = new WordLists(config.Verbs, config.Adjectives, config.Nouns);

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
            totalQuestions = WordLists.Verbs.Count;
        }
        currentQuestion = 0;
        amountCorrect = 0;
        
        StrictMode = config.Strictmode;

        this.restartAction = restartAction;
        PrepareNextQuestion();
    }

    private ConjugationType GetRandomQuestionType(WordType wordtype)
    {
        return QuizUtility.GetRandomQuestionType(wordtype, conjugationTypes);
    }

    private void OnInputChanged()
    {
        confirmAnswer = false;
        textField.style.color = Color.black;
    }

    private void OnPressSubmit()
    {
        if (hintVisible)
        {
            ToggleHint();
        }

        if (textField.value == "")
        {
            feedbackText = $"Please submit an Answer";
            textField.style.color = Color.maroon;
            textField.Focus();
            return;
        }
        if (currentConjugationType != ConjugationType.Meaning)
        {
            if (textField.value.ContainsEnglishCharacters())
            {
                feedbackText = $"Text must only contain japanese characters";
                textField.style.color = Color.maroon;
                textField.Focus();
                return;
            }
        }
        else
        {
            if (textField.value.ContainsEnglishCharacters() == false)
            {
                feedbackText = $"please answer in english";
                textField.style.color = Color.maroon;
                textField.Focus();
                return;
            }
        }



        if (shouldConfirmAnswer == false)
        {
            if (CheckAnswer(textField.value))
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
            if (CheckAnswer(textField.value))
            {
                feedbackText = "Correct!";
                amountCorrect += 1;
                AnswerSubmitted?.Invoke(true);
            }
            else
            {
                feedbackText = $"The correct answer was {GetAnswer()}.";
                AnswerSubmitted?.Invoke(false);
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

    private bool CheckAnswer(string userAnswer)
    {
        return QuizUtility.CheckAnswer(userAnswer, currentAnswer, currentConjugationType);
    }

    private string GetAnswer()
    {
        return QuizUtility.GetAnswer(currentAnswer);
    }

    private bool shouldConfirmAnswer => StrictMode || confirmAnswer ? true : false;

    private void PrepareNextQuestion()
    {
        if (currentQuestion >= totalQuestions) 
        {
            EndQuiz();
            return;
        }

        Question question = GetQuestion();

        (List<string> answer, string type) questionData = QuizUtility.GetQuestionAndAnswer(question, WordLists);

        int wordIndex = question.Index;
        ConjugationType form = question.conjugationType;
        WordType wordType = question.Wordtype;

        if (form == ConjugationType.Meaning)
        {
            textConverter.SetEnabled(false);
        }
        else
        {
            textConverter.SetEnabled(true);
        }

        questionType = questionData.type;
        currentAnswer = questionData.answer;

        if (wordType == WordType.Verb)
        {
            currentKanji = WordLists.Verbs[wordIndex].Kanji;
            currentKana = WordLists.Verbs[wordIndex].Kana;
            askedQuestions.Add((WordLists.Verbs[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Adjective)
        {
            currentKanji = WordLists.Adjectives[wordIndex].Kanji;
            currentKana = WordLists.Adjectives[wordIndex].Kana;
            askedQuestions.Add((WordLists.Adjectives[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Noun)
        {
            currentKanji = WordLists.Nouns[wordIndex].Kanji;
            currentKana = WordLists.Nouns[wordIndex].Kana;
            askedQuestions.Add((WordLists.Nouns[wordIndex].Kana, form));
        }

        textField.Focus();
    }

    private Question GetQuestion()
    {
        WordType wordType = QuizUtility.GetRandomWordType(WordLists);
        ConjugationType form = GetRandomQuestionType(wordType);

        currentWordType = wordType;
        currentConjugationType = form;
        
        int wordIndex = QuizUtility.GetRandomWordIndex(WordLists, wordType);
        if (wordType == WordType.Verb)
        {
            if (askedQuestions.Contains((WordLists.Verbs[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = WordLists.Verbs[wordIndex];
        }
        else if (wordType == WordType.Adjective)
        {
            if (askedQuestions.Contains((WordLists.Adjectives[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = WordLists.Adjectives[wordIndex];
        }
        else if (wordType == WordType.Noun)
        {
            if (askedQuestions.Contains((WordLists.Nouns[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = WordLists.Nouns[wordIndex];
        }

        return new Question(wordIndex, wordType, form);
    }

    private void EndQuiz()
    {
        feedbackText += $" \n \n Quiz Complete! \n {amountCorrect} / {totalQuestions}.";
        restartButton.SetEnabled(true);
        submitButton.SetEnabled(false);
        textField.SetEnabled(false);
        restartButton.visible = true;
        hintButton.SetEnabled(false);
    }

    private void RestartQuiz()
    {
        if (restartAction != null)
        {
            submitButton.SetEnabled(true);
            textField.SetEnabled(true);
            restartAction.Invoke();
            hintButton.SetEnabled(true);
            restartButton.visible = false;
        }
    }

    private void ToggleHint()
    {
        if (hintVisible)
        {
            hintVisible = false;
            hintButton.RemoveFromClassList("Pressed");
            feedbackText = "";
        }
        else
        {
            hintVisible = true;
            hintButton.AddToClassList("Pressed");
            feedbackText = Hint.GetHint(currentWord, currentConjugationType);
            hintButton.Focus();
        }
    }

    private void OnDestroy()
    {
        if (Initialized)
        {
            Unsubscribe();
        }
    }
}
