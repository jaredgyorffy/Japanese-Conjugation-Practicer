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
    private Button hintButton;
    private TextField textField;
    private Action restartAction;
    private KanaRomajiTranslator textConverter;

    private string currentAnswer;
    private int amountCorrect;

    private List<ConjugationType> VerbConjugationTypes;
    private List<ConjugationType> AdjectiveConjugationTypes;
    private List<ConjugationType> NounConjugationTypes;

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

    public List<Verb> Verbs;
    public List<Adjective> Adjectives;
    public List<Noun> Nouns;

    private List<(string, ConjugationType)> askedQuestions = new();
    
    private bool confirmAnswer = false;
    private bool hintVisible = false;
        
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
        

        hintButton = root.MQ<Button>("Hint");
        hintButton.clicked += ToggleHint;
        
        textConverter = GetComponent<KanaRomajiTranslator>();
        textConverter.InputChanged += OnInputChanged;
        
        textField.RegisterCallback<BlurEvent>(evt =>
        {
            if (MobileKeyboardInput.CheckInput() == TouchScreenKeyboard.Status.Done)
            {
                evt.StopImmediatePropagation();
                OnPressSubmit();
            }
        });
        
        textField.RegisterCallback<NavigationSubmitEvent>(evt =>
        {
            evt.StopImmediatePropagation();
            OnPressSubmit();
        }, TrickleDown.TrickleDown);
    }

    private void InitializeQuestionTypes(QuizConfiguration config)
    {
        VerbConjugationTypes = new List<ConjugationType>();
        if (config.VerbPoliteNonpastForm)
        {
            VerbConjugationTypes.Add(ConjugationType.PoliteNonpast);
        }
        if (config.VerbPoliteNonpastNegativeForm)
        {
            VerbConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.VerbPolitePastForm)
        {
            VerbConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.VerbPolitePastNegativeForm)
        {
            VerbConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.VerbStandardPastForm)
        {
            VerbConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.VerbStandardNonpastNegativeForm)
        {
            VerbConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.VerbStandardPastNegativeForm)
        {
            VerbConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.VerbPoliteVolitionalForm)
        {
            VerbConjugationTypes.Add(ConjugationType.PoliteVolitional);
        }
        if (config.VerbTeForm)
        {
            VerbConjugationTypes.Add(ConjugationType.TeForm);
        }

        AdjectiveConjugationTypes = new List<ConjugationType>();
        if (config.AdjectivePoliteNonpastNegativeForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.AdjectivePolitePastForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.AdjectivePolitePastNegativeForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.AdjectiveStandardPastForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.AdjectiveStandardNonpastNegativeForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.AdjectiveStandardPastNegativeForm)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }

        NounConjugationTypes = new List<ConjugationType>();
        if (config.NounPoliteNonpastNegativeForm)
        {
            NounConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.NounPolitePastForm)
        {
            NounConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.NounPolitePastNegativeForm)
        {
            NounConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.NounStandardPastForm)
        {
            NounConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.NounStandardNonpastNegativeForm)
        {
            NounConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.NounStandardPastNegativeForm)
        {
            NounConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
    }

    public void InitializeQuiz(QuizConfiguration config, int QuestionCount = 0, Action restartAction = null)
    {
        Verbs = config.Verbs;
        Adjectives = config.Adjectives;
        Nouns = config.Nouns;

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
            totalQuestions = Verbs.Count;
        }
        currentQuestion = 0;
        amountCorrect = 0;
        
        StrictMode = config.Strictmode;

        this.restartAction = restartAction;
        PrepareNextQuestion();
    }

    private ConjugationType GetQuestionType(WordType wordtype)
    {
        if (wordtype == WordType.Verb)
        {
            int index = UnityEngine.Random.Range(0, VerbConjugationTypes.Count);
            return VerbConjugationTypes[index];
        }
        else if (wordtype == WordType.Adjective)
        {
            int index = UnityEngine.Random.Range(0, AdjectiveConjugationTypes.Count);
            Debug.Log(index);
            return AdjectiveConjugationTypes[index];
        }
        else if (wordtype == WordType.Noun)
        {
            int index = UnityEngine.Random.Range(0, NounConjugationTypes.Count);
            Debug.Log(index);
            return NounConjugationTypes[index];
        }
        else
        {
            Debug.LogWarning("Unable to select question type because invalid wordtype was supplied");
            return 0;
        }

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
                feedbackText = "Correct!";
                //TODO: Add Correct VFX
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

        (int, WordType wordtype, ConjugationType) question = GetQuestion();

        int wordIndex = question.Item1;
        ConjugationType form = question.Item3;
        WordType wordType = question.wordtype;

        if (wordType == WordType.Verb)
        {
            switch (form)
            {
            case ConjugationType.PoliteNonpast:
                currentAnswer = Verbs[wordIndex].PoliteNonpast;
                questionType = "Polite Non-past Form";
                break;
            case ConjugationType.PoliteNonpastNegative:
                currentAnswer = Verbs[wordIndex].PoliteNonPastNegative;
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                currentAnswer = Verbs[wordIndex].PolitePast;
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                currentAnswer = Verbs[wordIndex].PolitePastNegative;
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                currentAnswer = Verbs[wordIndex].StandardPast;
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpast:
                currentAnswer = Verbs[wordIndex].StandardNonpast;
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                currentAnswer = Verbs[wordIndex].StandardNonpastNegative;
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                currentAnswer = Verbs[wordIndex].StandardPastNegative;
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.PoliteVolitional:
                currentAnswer = Verbs[wordIndex].PoliteVolitional;
                questionType = "Polite Volitional Form";
                break;
            case ConjugationType.TeForm:
                currentAnswer = Verbs[wordIndex].TeForm;
                questionType = "Te-form";
                break;
            case ConjugationType.CasualVolitional:
                currentAnswer = Verbs[wordIndex].CasualVolitional;
                questionType = "Casual Volitional Form";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
            currentKanji = Verbs[wordIndex].Kanji;
            currentKana = Verbs[wordIndex].Kana;
            askedQuestions.Add((Verbs[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Adjective)
        {
            switch (form)
            {
            case ConjugationType.PoliteNonpastNegative:
                currentAnswer = Adjectives[wordIndex].PoliteNonpastNegative;
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                currentAnswer = Adjectives[wordIndex].PolitePast;
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                currentAnswer = Adjectives[wordIndex].PolitePastNegative;
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                currentAnswer = Adjectives[wordIndex].StandardPast;
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                currentAnswer = Adjectives[wordIndex].StandardNonpastNegative;
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                currentAnswer = Adjectives[wordIndex].StandardPastNegative;
                questionType = "Standard Past Negative Form";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
            currentKanji = Adjectives[wordIndex].Kanji;
            currentKana = Adjectives[wordIndex].Kana;
            askedQuestions.Add((Adjectives[wordIndex].Kana, form));
        }
        else if (wordType == WordType.Noun)
        {
            switch (form)
            {
            case ConjugationType.PoliteNonpastNegative:
                currentAnswer = Nouns[wordIndex].PoliteNonpastNegative;
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                currentAnswer = Nouns[wordIndex].PolitePast;
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                currentAnswer = Nouns[wordIndex].PolitePastNegative;
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                currentAnswer = Nouns[wordIndex].StandardPast;
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                currentAnswer = Nouns[wordIndex].StandardNonpastNegative;
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                currentAnswer = Nouns[wordIndex].StandardPastNegative;
                questionType = "Standard Past Negative Form";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
            currentKanji = Nouns[wordIndex].Kanji;
            currentKana = Nouns[wordIndex].Kana;
            askedQuestions.Add((Nouns[wordIndex].Kana, form));
        }


        textField.Focus();
    }

    private (int, WordType wordtype, ConjugationType) GetQuestion()
    {
        WordType wordtype;
        if (Verbs.Count > 0 && Adjectives.Count == 0 && Nouns.Count == 0)
        {
            wordtype = WordType.Verb;
        }
        else if (Verbs.Count == 0 && Adjectives.Count > 0 && Nouns.Count == 0)
        {
            wordtype = WordType.Adjective;
        }
        else if (Verbs.Count == 0 && Adjectives.Count == 0 && Nouns.Count > 0)
        {
            wordtype = WordType.Noun;
        }
        else if (Verbs.Count > 0 && Adjectives.Count > 0 && Nouns.Count == 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Verb : WordType.Adjective;
        }
        else if (Verbs.Count == 0 && Adjectives.Count > 0 && Nouns.Count > 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Noun : WordType.Adjective;
        }
        else if (Verbs.Count > 0 && Adjectives.Count == 0 && Nouns.Count > 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Noun : WordType.Verb;
        }
        else
        {
            int random = UnityEngine.Random.Range(0, 3);
            if (random == 0)
            {
                wordtype = WordType.Noun;
            }
            else if (random == 1)
            {
                wordtype = WordType.Verb;
            }
            else
            {
                wordtype = WordType.Adjective;
            }
        }

        Debug.Log(wordtype);

        ConjugationType form = GetQuestionType(wordtype);

        currentWordType = wordtype;
        currentConjugationType = form;
        
        int wordIndex = 0;
        if (wordtype == WordType.Verb)
        {
            wordIndex = UnityEngine.Random.Range(0, Verbs.Count);
            if (askedQuestions.Contains((Verbs[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = Verbs[wordIndex];
        }
        else if (wordtype == WordType.Adjective)
        {
            wordIndex = UnityEngine.Random.Range(0, Adjectives.Count);
            if (askedQuestions.Contains((Adjectives[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = Adjectives[wordIndex];
        }
        else if (wordtype == WordType.Noun)
        {
            wordIndex = UnityEngine.Random.Range(0, Nouns.Count);
            if (askedQuestions.Contains((Nouns[wordIndex].Kana, form)))
            {
                return GetQuestion();
            }
            currentWord = Nouns[wordIndex];
        }

        return (wordIndex, wordtype, form);
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
        submitButton.clicked -= OnPressSubmit;
        textConverter.InputChanged -= OnInputChanged;
    }
}
