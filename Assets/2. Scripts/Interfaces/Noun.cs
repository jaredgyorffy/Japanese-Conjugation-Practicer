using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class Noun : IWord
{
    public string Kana => kana;
    [SerializeField] private string kana;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public WordType WordType => WordType.Noun;
    public List<string> Meaning => meaning;
    [SerializeField] private List<string> meaning;

    public string StandardNonpast => kana;
    public string PolitePast => ConjugatePolitePast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string politePast;

    public string StandardPast => ConjugateStandardPast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string standardPast;

    public string StandardNonpastNegative => ConjugateStandardNonpastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string standardNonpastNegative;

    public string PoliteNonpastNegative => ConjugatePoliteNonpastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string politeNonpastNegative;
    
    public string PolitePastNegative => ConjugatePolitePastNegative(kana);

    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string politePastNegative;

    public string StandardPastNegative => ConjugateStandardPastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string standardPastNegative;

    private string ConjugatePolitePast(string dictionaryForm)
    {
        return dictionaryForm + "でした";
    }

    private string ConjugateStandardPast(string dictionaryForm)
    {
        return dictionaryForm + "だった";
    }

    private string ConjugateStandardNonpastNegative(string dictionaryForm)
    {
        return dictionaryForm + "じゃない";
    }

    private string ConjugatePoliteNonpastNegative(string dictionaryForm)
    {
        return dictionaryForm + "じゃありません";
    }

    private string ConjugatePolitePastNegative(string dictionaryForm)
    {
        return dictionaryForm + "じゃありませんでした";
    }

    private string ConjugateStandardPastNegative(string dictionaryForm)
    {
        return dictionaryForm + "じゃなかった";
    }
}