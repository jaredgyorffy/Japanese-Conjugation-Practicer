using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;
using Unity.VisualScripting;
[Serializable]
public class Verb : IWord
{
    public WordType WordType => WordType.Verb;

    public VerbType VerbType => verbType;
    [SerializeField] private VerbType verbType;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public string Kana => kana;
    [SerializeField] private string kana;

    public List <string> Meaning => meaning;
    [SerializeField] private List<string> meaning;
    public string PoliteNonpast => ConjugatePoliteNonpast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politeNonpast;

    public string PolitePast => ConjugatePolitePast(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politePast;

    public string PoliteNonPastNegative => ConjugatePoliteNonPastNegative(kana);
    [Foldout("Irregular Conjugation")][ShowIf("VerbType", VerbType.IRR)][SerializeField][AllowNesting] private string politeNonPastNegative;

    [Foldout("Tenses")][FormerlySerializedAs("PastNegative")] public string PolitePastNegative;
    [Foldout("Tenses")] public string StandardNonpast;
    [Foldout("Tenses")][FormerlySerializedAs("ShortPast")] public string StandardPast;
    [Foldout("Tenses")][FormerlySerializedAs("ShortPresentNegative")] public string StandardNonpastNegative;
    [Foldout("Tenses")][FormerlySerializedAs("ShortPastNegative")] public string StandardPastNegative;
    [Foldout("Tenses")][FormerlySerializedAs("Volitional")] public string PoliteVolitional;
    [Foldout("Tenses")] public string StandardVolitional;
    [Foldout("Tenses")][FormerlySerializedAs("TeForm")] public string TeForm;
    
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
        return conjugatedForm;
    }
}

public enum VerbType
{
    U, //Godan Verbs, U-Verbs
    RU, // Ichidan Verbs, RU-Verbs
    IRR, // 
}

//Formality > Time > Positive/Negative
public enum VerbConjugation
{
    StandardNonpast,
    PoliteNonpast,
    PoliteNonpastNegative,
    PolitePast,
    PolitePastNegative,
    StandardNonpastNegative,
    StandardPast,
    StandardPastNegative,
    PoliteVolitional,
    StandardVolitional,
    TeForm,
}