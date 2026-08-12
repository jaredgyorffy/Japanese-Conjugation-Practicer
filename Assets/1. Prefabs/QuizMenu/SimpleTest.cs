using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
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

    private List<string> currentAnswer = new();
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
        if (config.VerbMeaning)
        {
            VerbConjugationTypes.Add(ConjugationType.Meaning);
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
        if (config.AdjectiveMeaning)
        {
            AdjectiveConjugationTypes.Add(ConjugationType.Meaning);
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
        if (config.NounMeaning)
        {
            NounConjugationTypes.Add(ConjugationType.Meaning);
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
            return AdjectiveConjugationTypes[index];
        }
        else if (wordtype == WordType.Noun)
        {
            int index = UnityEngine.Random.Range(0, NounConjugationTypes.Count);
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
            if (textField.value == currentAnswer[0])
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
            }
            else
            {
                feedbackText = $"The correct answer was {GetAnswer()}.";
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

    private bool CheckAnswer(string Answer)
    {
        if (currentConjugationType == ConjugationType.Meaning)
        {
            foreach (string answer in currentAnswer)
            {
                double score = Fuzzy.Ratio(Answer, answer);
                if (score > 0.85)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return (Answer == GetAnswer());
        }
    }

    private void SetAnswer(string answer)
    {
        currentAnswer = new();
        currentAnswer.Add(answer);
    }
    private void SetAnswer(List<string> answers)
    {
        currentAnswer = answers;
    }

    private string GetAnswer()
    {
        if (currentAnswer.Count > 1)
        {
            string answers = "";
            for (int i = 0; i < currentAnswer.Count; i++)
            {
                answers += currentAnswer[i];
                if (i < currentAnswer.Count - 1)
                {
                    answers += " or ";
                }
            }

            return answers;
        }
        else
        {
            return currentAnswer[0];
        }
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
            textConverter.SetEnabled(true);
            switch (form)
            {
            case ConjugationType.PoliteNonpast:
                SetAnswer(Verbs[wordIndex].PoliteNonpast);
                questionType = "Polite Non-past Form";
                break;
            case ConjugationType.PoliteNonpastNegative:
                SetAnswer(Verbs[wordIndex].PoliteNonPastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                SetAnswer(Verbs[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                SetAnswer(Verbs[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                SetAnswer(Verbs[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpast:
                SetAnswer(Verbs[wordIndex].StandardNonpast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                SetAnswer(Verbs[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                SetAnswer(Verbs[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.PoliteVolitional:
                SetAnswer(Verbs[wordIndex].PoliteVolitional);
                questionType = "Polite Volitional Form";
                break;
            case ConjugationType.TeForm:
                SetAnswer(Verbs[wordIndex].TeForm);
                questionType = "Te-form";
                break;
            case ConjugationType.CasualVolitional:
                SetAnswer(Verbs[wordIndex].CasualVolitional);
                questionType = "Casual Volitional Form";
                break;
            case ConjugationType.Meaning:
                SetAnswer(Verbs[wordIndex].Meaning);
                questionType = "Meaning";
                textConverter.SetEnabled(false);
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
            textConverter.SetEnabled(true);
            switch (form)
            {

            case ConjugationType.PoliteNonpastNegative:
                SetAnswer(Adjectives[wordIndex].PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                SetAnswer(Adjectives[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                SetAnswer(Adjectives[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                SetAnswer(Adjectives[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                SetAnswer(Adjectives[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                SetAnswer(Adjectives[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.Meaning:
                SetAnswer(Adjectives[wordIndex].Meaning);
                questionType = "Meaning";
                textConverter.SetEnabled(false);
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
            textConverter.SetEnabled(true);
            switch (form)
            {
            case ConjugationType.PoliteNonpastNegative:
                SetAnswer(Nouns[wordIndex].PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                SetAnswer(Nouns[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                SetAnswer(Nouns[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                SetAnswer(Nouns[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                SetAnswer(Nouns[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                SetAnswer(Nouns[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.Meaning:
                SetAnswer(Nouns[wordIndex].Meaning);
                questionType = "Meaning";
                textConverter.SetEnabled(false);
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
