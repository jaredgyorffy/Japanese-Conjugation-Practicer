using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;
[Serializable]
public class Verb :  IWord
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

    [Foldout("Tenses")][FormerlySerializedAs("Present")] public string Present;
    [Foldout("Tenses")][FormerlySerializedAs("Negative")] public string PresentNegative;
    [Foldout("Tenses")] public string Past;
    [Foldout("Tenses")] public string PastNegative;
    [Foldout("Tenses")] public string ShortPast;
    [Foldout("Tenses")] public string ShortPresentNegative;
    [Foldout("Tenses")] public string ShortPastNegative;
    [Foldout("Tenses")] public string Volitional;
    [Foldout("Tenses")] public string TeForm;
    [Foldout("Tenses")] public string Request;
}

public enum VerbType
{
    U, //Godan Verbs, U-Verbs
    RU, // Ichidan Verbs, RU-Verbs
    IRR, // 
}

public enum VerbConjugation
{
    Dictionary,
    Present,
    PresentNegative,
    Past,
    PastNegative,
    ShortPast,
    ShortPresentNegative,
    ShortPastNegative,
    Volitional,
    TeForm,
    Request,
}