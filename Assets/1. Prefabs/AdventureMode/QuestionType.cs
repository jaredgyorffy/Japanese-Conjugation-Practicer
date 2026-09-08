
using System;

[Serializable]
public class QuestionType
{
    public string Title;
    public QuestionCategory Category;
    public ConjugationTypes ConjugationTypes;
}

public enum QuestionCategory
{
    None = 0,

    Polite = 100,
    Standard = 101,


    Past = 200,
    Negative = 201,
    Te = 203,


    Vocab = 300,
    Expression = 301,

    Verb = 401,
    Noun = 402,
    Adjective = 403,
    Adverb = 404,
    Particle = 405,

    Grammer = 500,
    Numbers = 501,
    KoSoADo = 502,

}