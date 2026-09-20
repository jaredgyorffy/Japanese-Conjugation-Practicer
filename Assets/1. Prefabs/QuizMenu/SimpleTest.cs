using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTest : MonoBehaviour, IQuiz
{
    QuizMenu quizMenu;
    public event Action<bool, string> AnswerSubmitted;
    public event Action NextQuestion;

    private QuestionType questionType;
    private Question currentQuestion;

    public WordLists WordLists;

    private List<Question> askedQuestions = new();

    private bool confirmAnswer = false;

    private bool StrictMode = false;
    private bool useKanaKeyboard;

    public bool Initialized { get; private set; }

    public void SetQuestionType(QuestionType types)
    {
        questionType = types;

        if (useKanaKeyboard == false)
        {
            return;
        }

        /*if (types.Category == QuestionCategory.Expression || types.Category == QuestionCategory.Vocab)
        {
            quizMenu.SetKanaKeyboard(false);
        }
        else
        {
            quizMenu.SetKanaKeyboard(true);
        }*/
    }

    public void InitializeQuiz(QuizConfiguration config, QuizMenu quizmenu)
    {
        WordLists = new WordLists(config.Verbs, config.Adjectives, config.Nouns, config.Expressions, config.Adverbs, config.Grammers);
        quizMenu = quizmenu;
        quizMenu.SetNumberVisible(false);
        askedQuestions = new();
        SetInformationText("");
        useKanaKeyboard = false;
        //useKanaKeyboard = config.UseKanaKeyboard;
        quizMenu.SetKanaKeyboard(useKanaKeyboard);
        StrictMode = config.Strictmode;
    }

    public void SetInformationText(string text)
    {
        quizMenu.SetInformationText(text);
    }

    public (bool, string) OnPressSubmit(string answer)
    {
        ToggleHint(false);

        if (shouldConfirmAnswer == false)
        {
            if (CheckAnswer(answer))
            {
                SetInformationText("Correct");
                confirmAnswer = true;
                return (true, "");
            }
            else
            {
                SetInformationText("Wrong! Try again?");
                confirmAnswer = true;
                return (false, "string");
            }
        }
        else
        {
            confirmAnswer = false;
            if (CheckAnswer(answer))
            {
                SetInformationText("");
                AnswerSubmitted?.Invoke(true, "");

                return (true, "");
            }
            else
            {
                SetInformationText("");
                AnswerSubmitted?.Invoke(false, "frogs");
                return (false, GetAnswer());
            }
        }
    }

    public bool CheckAnswer(string userAnswer)
    {
        return QuizUtility.CheckAnswer(userAnswer, currentQuestion);
    }

    private string GetAnswer()
    {
        return QuizUtility.GetAnswer(currentQuestion.Answers);
    }

    private bool shouldConfirmAnswer => StrictMode || confirmAnswer ? true : false;

    public void PrepareNextQuestion()
    {
        askedQuestions.Add(currentQuestion);
        Question question = GetQuestion();
        currentQuestion = question;
        quizMenu.SetQuestion(question.QuestionText);

        SetKana(question.Word);
    }

    private void SetKana(string kana, string Kanji)
    {
        quizMenu.SetKana(kana, Kanji);
    }

    private void SetKana(IWord word)
    {
        quizMenu.SetKana(word.Kana, word.Kanji);
    }

    private Question GetQuestion()
    {
        switch (questionType.Category)
        {
        case QuestionCategory.Conjugation:
            return GetConjugationQuesiton();
        case QuestionCategory.Grammer:
            return GetGrammerQuestion();
        case QuestionCategory.Expression:
            return GetExpressionQuestion();
        case QuestionCategory.Vocab:
            return GetVocabQuestion();
        default:
            Debug.LogError($"invalid Question Category {questionType.Category}");
            break;
        }
        return GetConjugationQuesiton();
    }

    private Question GetVocabQuestion()
    {
        return QuizUtility.GetVocabQuestion(ref WordLists);
    }

    private Question GetExpressionQuestion()
    {
        IWord word = QuizUtility.GetRandomWord(ref WordLists, WordType.Expression);
        if (word == null)
        {
            Debug.LogError("Ran out of expressions");
        }
        return QuizUtility.GetExpressionQuestion(word);
    }

    private Question GetConjugationQuesiton()
    {
        List<string> answers = new List<string>();

        WordType wordType = QuizUtility.GetRandomWordType(questionType.ConjugationTypes);
        ConjugationType form = questionType.ConjugationTypes.GetConjugationTypeByWordType(wordType);
        IWord word = QuizUtility.GetRandomWord(ref WordLists, wordType);

        if (word == null)
        {
            Debug.LogError($"Ran out of {wordType.ToString()}");
        }

        return QuizUtility.GetConjugationQuestion(wordType, form, word);
    }

    private Question GetGrammerQuestion()
    {
        return QuizUtility.GetRandomGrammer(WordLists);
    }

    public void ToggleHint(bool hintVisible)
    {
        if (hintVisible)
        {
            SetInformationText("");
        }
        else
        {
            SetInformationText(currentQuestion.Hint);
        }
    }
}
