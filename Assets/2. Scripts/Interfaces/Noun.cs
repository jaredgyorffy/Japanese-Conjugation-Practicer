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

    public string StandardPast => ConjugateStandardPast(kana);

    public string StandardNonpastNegative => ConjugateStandardNonpastNegative(kana);

    public string StandardPastNegative => ConjugateStandardPastNegative(kana);

    public string PoliteNonpastNegative => ConjugatePoliteNonpastNegative(kana);
    
    public string PolitePastNegative => ConjugatePolitePastNegative(kana);

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