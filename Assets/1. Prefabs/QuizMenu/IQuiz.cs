
using System;

public interface IQuiz
{
    public void InitializeQuiz(QuizConfiguration config, QuizMenu menu);
    public void SetQuestionType(QuestionType type);
    public void SetInformationText(string text);
    public (bool, string) OnPressSubmit(string answer);
    public bool CheckAnswer(string userAnswer);
    public void PrepareNextQuestion();
    public void ToggleHint(bool hintVisible);
}
