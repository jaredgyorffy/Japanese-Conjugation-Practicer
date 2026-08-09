using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class Adjective : IWord
{
    public string Kana => kana;
    [SerializeField] private string kana;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public WordType WordType => WordType.Adjective;
    public AdjectiveType AdjectiveType => adjectiveType;
    [SerializeField] private AdjectiveType adjectiveType;
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
    
    public string PolitePastNegative => ConjugateStandardPastNegative(kana);

    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string politePastNegative;

    public string StandardPastNegative => ConjugatePolitePastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("AdjectiveType", AdjectiveType.IRR)][SerializeField][AllowNesting] private string standardPastNegative;

    private string ConjugatePolitePast(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "かった";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm + "でした";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return politePast;
        }
        return conjugatedForm;
    }

    private string ConjugateStandardPast(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "かった";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "だった";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return standardPast;
        }
        return conjugatedForm;
    }

    private string ConjugateStandardNonpastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "くない";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm + "じゃない";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return standardNonpastNegative;
        }
        return conjugatedForm;
    }

    private string ConjugatePoliteNonpastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "くありません";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm + "じゃありません";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return standardNonpastNegative;
        }
        return conjugatedForm;
    }

    private string ConjugatePolitePastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "くありませんでした";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm + "じゃありませんでした";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return standardNonpastNegative;
        }
        return conjugatedForm;
    }

    private string ConjugateStandardPastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (AdjectiveType == AdjectiveType.I)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "くなかった";
        }
        else if (AdjectiveType == AdjectiveType.NA)
        {
            conjugatedForm = dictionaryForm + "じゃなかった";
        }
        else if (AdjectiveType == AdjectiveType.IRR)
        {
            return standardNonpastNegative;
        }
        return conjugatedForm;
    }
}


public enum AdjectiveType
{
    I, //i-adjective
    NA, // na-adjective
    IRR, // irregular
}