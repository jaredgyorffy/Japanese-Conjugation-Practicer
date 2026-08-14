using UnityEngine;

public class Question
{
    public Question(int Index, WordType Wordtype, ConjugationType conjugationType)
    {
        this.Index = Index;
        this.Wordtype = Wordtype;
        this.conjugationType = conjugationType;
    }

    public int Index;
    public WordType Wordtype;
    public ConjugationType conjugationType;
}
