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
    [Foldout("Tenses")][FormerlySerializedAs("PresentNegative")] public string PoliteNonPastNegative;
    [Foldout("Tenses")][FormerlySerializedAs("Past")] public string PolitePast;
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
                case 'つ':
                case 'る':
                    conjugatedForm = stem + "います";
                    break;
                case 'む':
                case 'ぶ':
                case 'ぬ':
                    conjugatedForm = stem + "みます";
                    break;
                case 'く':
                    conjugatedForm = stem + "きます";
                    break;
                case 'ぐ':
                    conjugatedForm = stem + "ぎます";
                    break;
                case 'す':
                    conjugatedForm = stem + "します";
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