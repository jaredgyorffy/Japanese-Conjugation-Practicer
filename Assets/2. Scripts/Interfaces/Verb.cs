using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class Verb : IWord
{
    public string Kana => kana;
    [SerializeField] private string kana;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public WordType WordType => WordType.Verb;

    public VerbType VerbType => verbType;
    [SerializeField] private VerbType verbType;
    public string MeaningFull => meaningFull;
    [SerializeField] private string meaningFull;
    public List <string> Meaning => meaning;
    [SerializeField] private List<string> meaning;
    public string PoliteNonpast => ConjugatePoliteNonpast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politeNonpast;

    public string PolitePast => ConjugatePolitePast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politePast;

    public string PoliteNonPastNegative => ConjugatePoliteNonPastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politeNonPastNegative;

    public string PolitePastNegative => ConjugatePolitePastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politePastNegative;
    public string StandardNonpast => kana;

    public string StandardPast => ConjugateStandardpast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string standardPast;

    public string StandardNonpastNegative => ConjugateStandardNonPastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string standardNonpastNegative;

    public string StandardPastNegative => ConjugateStandardPastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string standardPastNegative;
    public string PoliteVolitional => ConjugatePoliteVolitional(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politeVolitional;

    public string CasualVolitional => ConjugateCasualVolitional(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private  string casualVolitional;
    public string TeForm => ConjugateTeForm(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string teForm;

    private string ConjugatePoliteNonpast(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ます";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "います";
                break;
            case 'く':
                conjugatedForm = stem + "きます";
                break;
            case 'す':
                conjugatedForm = stem + "します";
                break;
            case 'つ':
                conjugatedForm = stem + "ちます";
                break;
            case 'る':
                conjugatedForm = stem + "ります";
                break;
            case 'ぬ':
                conjugatedForm = stem + "にます";
                break;
            case 'む':
                conjugatedForm = stem + "みます";
                break;
            case 'ぶ':
                conjugatedForm = stem + "びます";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ぎます";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return politeNonpast;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "します";
        }

        return conjugatedForm;
    }
    private string ConjugatePoliteNonPastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ません";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "いません";
                break;
            case 'く':
                conjugatedForm = stem + "きません";
                break;
            case 'す':
                conjugatedForm = stem + "しません";
                break;
            case 'つ':
                conjugatedForm = stem + "ちません";
                break;
            case 'る':
                conjugatedForm = stem + "りません";
                break;
            case 'ぬ':
                conjugatedForm = stem + "にません";
                break;
            case 'む':
                conjugatedForm = stem + "みません";
                break;
            case 'ぶ':
                conjugatedForm = stem + "びません";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ぎません";
                break;
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return politeNonPastNegative;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しません";
        }
        return conjugatedForm;
    }
    private string ConjugatePolitePast(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ました";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "いました";
                break;
            case 'く':
                conjugatedForm = stem + "きました";
                break;
            case 'す':
                conjugatedForm = stem + "しました";
                break;
            case 'つ':
                conjugatedForm = stem + "ちました";
                break;
            case 'る':
                conjugatedForm = stem + "りました";
                break;
            case 'ぬ':
                conjugatedForm = stem + "にました";
                break;
            case 'む':
                conjugatedForm = stem + "みました";
                break;
            case 'ぶ':
                conjugatedForm = stem + "びました";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ぎました";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return politePast;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しました";
        }
        return conjugatedForm;
    }
    private string ConjugatePolitePastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ませんでした";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "いませんでした";
                break;
            case 'く':
                conjugatedForm = stem + "きませんでした";
                break;
            case 'す':
                conjugatedForm = stem + "しませんでした";
                break;
            case 'つ':
                conjugatedForm = stem + "ちませんでした";
                break;
            case 'る':
                conjugatedForm = stem + "りませんでした";
                break;
            case 'ぬ':
                conjugatedForm = stem + "にませんでした";
                break;
            case 'む':
                conjugatedForm = stem + "みませんでした";
                break;
            case 'ぶ':
                conjugatedForm = stem + "びませんでした";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ぎませんでした";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return politePastNegative;
        }
        return conjugatedForm;
    }
    private string ConjugateStandardpast(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "た";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "った";
                break;
            case 'く':
                conjugatedForm = stem + "いた";
                break;
            case 'す':
                conjugatedForm = stem + "した";
                break;
            case 'つ':
                conjugatedForm = stem + "った";
                break;
            case 'る':
                conjugatedForm = stem + "った";
                break;
            case 'ぬ':
                conjugatedForm = stem + "んだ";
                break;
            case 'む':
                conjugatedForm = stem + "んだ";
                break;
            case 'ぶ':
                conjugatedForm = stem + "んだ";
                break;
            case 'ぐ':
                conjugatedForm = stem + "いだ";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return standardPast;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "した";
        }
        return conjugatedForm;
    }
    private string ConjugateStandardNonPastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ない";
        }
        else if (VerbType == VerbType.U)
        {
            char lastChar = dictionaryForm[^1];
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length -1);

            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "わない";
                break;
            case 'く':
                conjugatedForm = stem + "かない";
                break;
            case 'す':
                conjugatedForm = stem + "さない";
                break;
            case 'つ':
                conjugatedForm = stem + "たない";
                break;
            case 'る':
                conjugatedForm = stem + "らない";
                break;
            case 'ぬ':
                conjugatedForm = stem + "なない";
                break;
            case 'む':
                conjugatedForm = stem + "まない";
                break;
            case 'ぶ':
                conjugatedForm = stem + "ばない";
                break;
            case 'ぐ':
                conjugatedForm = stem + "がない";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return standardNonpastNegative;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しない";
        }
        return conjugatedForm;
    }
    private string ConjugateStandardPastNegative(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "なかった";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "わなかった";
                break;
            case 'く':
                conjugatedForm = stem + "かなかった";
                break;
            case 'す':
                conjugatedForm = stem + "さなかった";
                break;
            case 'つ':
                conjugatedForm = stem + "たなかった";
                break;
            case 'る':
                conjugatedForm = stem + "らなかった";
                break;
            case 'ぬ':
                conjugatedForm = stem + "ななかった";
                break;
            case 'む':
                conjugatedForm = stem + "まなかった";
                break;
            case 'ぶ':
                conjugatedForm = stem + "ばなかった";
                break;
            case 'ぐ':
                conjugatedForm = stem + "がなかった";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return standardNonpastNegative;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しなかった";
        }
        return conjugatedForm;
    }
    private string ConjugatePoliteVolitional(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "ましょう";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'う':
                conjugatedForm = stem + "いましょう";
                break;
            case 'く':
                conjugatedForm = stem + "きましょう";
                break;
            case 'す':
                conjugatedForm = stem + "しましょう";
                break;
            case 'つ':
                conjugatedForm = stem + "ちましょう";
                break;
            case 'る':
                conjugatedForm = stem + "りましょう";
                break;
            case 'ぬ':
                conjugatedForm = stem + "にましょう";
                break;
            case 'む':
                conjugatedForm = stem + "みましょう";
                break;
            case 'ぶ':
                conjugatedForm = stem + "びましょう";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ぎましょう";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return politeVolitional;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しましょう";
        }
        return conjugatedForm;
    }
    private string ConjugateCasualVolitional(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "よう";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'る':
                conjugatedForm = stem + "ろう";
                break;
            case 'う':
                conjugatedForm = stem + "おう";
                break;
            case 'く':
                conjugatedForm = stem + "こう";
                break;
            case 'す':
                conjugatedForm = stem + "そう";
                break;
            case 'つ':
                conjugatedForm = stem + "とう";
                break;
            case 'ぬ':
                conjugatedForm = stem + "のう";
                break;
            case 'ぶ':
                conjugatedForm = stem + "ぼう";
                break;
            case 'む':
                conjugatedForm = stem + "もう";
                break;
            case 'ぐ':
                conjugatedForm = stem + "ごう";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return casualVolitional;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "しよう";
        }
        return conjugatedForm;
    }
    private string ConjugateTeForm(string dictionaryForm)
    {
        string conjugatedForm = "";
        if (VerbType == VerbType.RU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 1) + "て";
        }
        else if (VerbType == VerbType.U)
        {
            string stem = dictionaryForm.Substring(0, dictionaryForm.Length - 1);
            char lastChar = dictionaryForm[dictionaryForm.Length - 1];
            switch (lastChar)
            {
            case 'る':
                conjugatedForm = stem + "って";
                break;
            case 'う':
                conjugatedForm = stem + "って";
                break;
            case 'つ':
                conjugatedForm = stem + "って";
                break;

            case 'く':
                conjugatedForm = stem + "いて";
                break;
            case 'ぐ':
                conjugatedForm = stem + "いて";
                break;

            case 'ぬ':
                conjugatedForm = stem + "んで";
                break;
            case 'ぶ':
                conjugatedForm = stem + "んで";
                break;
            case 'む':

                conjugatedForm = stem + "んで";
                break;
            case 'す':
                conjugatedForm = stem + "して";
                break;

            default:
                throw new Exception("Invalid verb ending for U-verb.");
            }
        }
        else if (VerbType == VerbType.IRR)
        {
            return teForm;
        }
        else if (VerbType == VerbType.SURU)
        {
            conjugatedForm = dictionaryForm.Substring(0, dictionaryForm.Length - 2) + "して";
        }
        return conjugatedForm;
    }
}

public enum VerbType
{
    U = 0, //Godan Verbs, U-Verbs
    RU = 1, // Ichidan Verbs, RU-Verbs
    IRR = 2, // Irregular
    SURU = 3, //Irregular Suru Type verbs.
}

public enum ConjugationType
{
    Meaning,
    StandardNonpast,
    PoliteNonpast,
    PoliteNonpastNegative,
    PolitePast,
    PolitePastNegative,
    StandardNonpastNegative,
    StandardPast,
    StandardPastNegative,
    PoliteVolitional,
    CasualVolitional,
    TeForm,
}