using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SimpleTest : MonoBehaviour
{
    [SerializeField] private UIDocument quizMenu;
    private VisualElement quizMenuRoot;
    private Button submitButton;
    private Button restartButton;
    private Button hintButton;
    private TextField textField;
    private KanaRomajiTranslator textConverter;
    private VisualElement informationBox;

    public event Action<bool, string> AnswerSubmitted;
    public event Action NextQuestion;

    private List<string> currentAnswer = new();

    private ConjugationTypes conjugationTypes;

    [CreateProperty] public string QuestionType => questionType;
    private string questionType;

    private IWord currentWord;
    private ConjugationType currentConjugationType;
    private WordType currentWordType;

    [CreateProperty] public string PreviousAnswer => informationText;
    private string informationText;

    [CreateProperty] public string CurrentKanji => currentKanji;
    private string currentKanji;

    [CreateProperty] public string CurrentKana => currentKana;
    private string currentKana;

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

        hintButton = quizMenuRoot.MQ<Button>("Hint");
        hintButton.clicked += ToggleHint;

        restartButton = quizMenuRoot.MQ<Button>("Restart");

        textConverter = quizMenu.GetComponent<KanaRomajiTranslator>();
        textConverter.InputChanged += OnInputChanged;
        informationBox = quizMenuRoot.MQ<VisualElement>("InformationBox");
        textField.RegisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);
        Initialized = true;
    }


    public void Unsubscribe()
    {
        if (Initialized)
        {
            submitButton.clicked -= OnPressSubmit;
            hintButton.clicked -= ToggleHint;
            textConverter.InputChanged -= OnInputChanged;
            textField.UnregisterCallback<NavigationSubmitEvent>(OnPressEnterToSubmit, TrickleDown.TrickleDown);
            Initialized = false;
        }
    }

    private void OnPressEnterToSubmit(NavigationSubmitEvent evt)
    {
        evt.StopImmediatePropagation();
        OnPressSubmit();
    }

    private void OnPressEnterToSubmit(KeyDownEvent evt)
    {
        evt.StopImmediatePropagation();
        OnPressSubmit();
    }

    private void Update()
    {
        if (Initialized == false)
        {
            return;
        }

        var keyboard = textField.touchScreenKeyboard;

        if (keyboard == null)
        {
            return;
        }

        if (keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            OnPressSubmit();
        }
    }



    private void InitializeQuestionTypes(QuizConfiguration config)
    {
        ConjugationTypes conjugation = QuizUtility.InitializeQuestionTypes(config);
        conjugationTypes = conjugation;
    }

    public void SetQuestionTypes(ConjugationTypes types)
    {
        conjugationTypes = types;
    }

    public void InitializeQuiz(QuizConfiguration config)
    {
        TryInitialize();
        WordLists = new WordLists(config.Verbs, config.Adjectives, config.Nouns);

        quizMenuRoot.MQ<VisualElement>("Number").visible = false;
        askedQuestions = new();
        restartButton.SetEnabled(true);
        restartButton.visible = false;
        SetInformationText("");
        InitializeQuestionTypes(config);
        
        StrictMode = config.Strictmode;
    }

    private void SetInformationText(string text)
    {
        informationText = text;
        informationBox.style.visibility = string.IsNullOrEmpty(text)
            ? Visibility.Hidden
            : Visibility.Visible;
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
            SetInformationText("Please submit an Answer");
            textField.style.color = Color.maroon;
            textField.Focus();
            return;
        }
        if (currentConjugationType != ConjugationType.Meaning)
        {
            if (textField.value.ContainsEnglishCharacters())
            {
                SetInformationText("Text must only contain japanese characters");
                textField.style.color = Color.maroon;
                textField.Focus();
                return;
            }
        }
        else
        {
            if (textField.value.ContainsEnglishCharacters() == false)
            {
                SetInformationText("please answer in english");
                textField.style.color = Color.maroon;
                textField.Focus();
                return;
            }
        }



        if (shouldConfirmAnswer == false)
        {
            if (CheckAnswer(textField.value))
            {
                SetInformationText("Correct");
                confirmAnswer = true;
                textField.Focus();
                textField.style.color = Color.forestGreen;
                return;
            }
            else
            {
                SetInformationText("Wrong! Try again?");
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
                SetInformationText("");
                AnswerSubmitted?.Invoke(true, "");
                CloseKeyboard();
            }
            else
            {
                informationText = "";
                AnswerSubmitted?.Invoke(false, GetAnswer());
                CloseKeyboard();
            }
        }

        textField.value = "";
        confirmAnswer = false;
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

    public void PrepareNextQuestion()
    {
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
            SetKana(WordLists.Verbs[wordIndex].Kana, WordLists.Verbs[wordIndex].Kanji);
            askedQuestions.Add((WordLists.Verbs[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Adjective)
        {
            SetKana(WordLists.Adjectives[wordIndex].Kana, WordLists.Adjectives[wordIndex].Kanji);
            askedQuestions.Add((WordLists.Adjectives[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Noun)
        {
            SetKana(WordLists.Nouns[wordIndex].Kana, WordLists.Nouns[wordIndex].Kanji);
            askedQuestions.Add((WordLists.Nouns[wordIndex].Kana, form));
        }

        textField.Focus();
    }

    private void SetKana(string kana, string Kanji)
    {
        if (string.IsNullOrEmpty(Kanji))
        {
            currentKanji = kana;
            currentKana = "";
        }
        else
        {
            currentKanji = kana;
            currentKana = Kanji;
        }

    }

    public void CloseKeyboard()
    {
        textField.Blur();
        quizMenuRoot.Focus();
        var keyboard = textField.touchScreenKeyboard;

        if (keyboard != null)
        {
            keyboard.active = false;
        }
    }

    private Question GetQuestion()
    {
        WordType wordType = QuizUtility.GetRandomWordType(conjugationTypes);
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

    private void ToggleHint()
    {
        if (hintVisible)
        {
            hintVisible = false;
            hintButton.RemoveFromClassList("Pressed");
            SetInformationText("");
        }
        else
        {
            hintVisible = true;
            hintButton.AddToClassList("Pressed");
            SetInformationText(Hint.GetHint(currentWord, currentConjugationType));
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
