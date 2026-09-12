using System;
using UnityEngine;

public class QuizMediator : MonoBehaviour
{
    [SerializeField] private QuizMenu quizMenu;
    public IQuiz CurrentQuiz {get; private set;}
    [SerializeField] SimpleTest defaultQuiz;
    private IQuiz vocabQuiz;
    public Action<bool, string> AnswerSubmitted;
    void Start()
    {
        quizMenu.TryInitialize();
        vocabQuiz = defaultQuiz;
        Initialize();
    }

    private void Initialize()
    {
        if (quizMenu)
        {
            quizMenu.HintPressed += OnHintPressed;
            quizMenu.SubmitButtonPressed += OnSubmitButtonPressed;
        }
    }

    public void InitializeQuiz(QuizConfiguration quiz)
    {
        CurrentQuiz.InitializeQuiz(quiz, quizMenu);
    }

    public void SetQuizType(QuestionCategory quesitonCategory)
    {
        switch(quesitonCategory)
        {
        default:
            CurrentQuiz = vocabQuiz;
            break;
        }
    }

    private void OnSubmitButtonPressed(string answer)
    {
        (bool, string) verdict = CurrentQuiz.OnPressSubmit(answer);
        AnswerSubmitted?.Invoke(verdict.Item1, verdict.Item2);
    }

    public void OnPrepareNextQuestion()
    {

    }

    private void OnHintPressed(bool hintVisible)
    {
        CurrentQuiz.ToggleHint(hintVisible);
    }
}
