using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
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

    [Foldout("Tenses")] public string Present;
    [Foldout("Tenses")] public string Negative;

    public bool IsExeception;
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
    Negative,
    Past,
    PastNegative,
    Volitional,
    TeForm,
}