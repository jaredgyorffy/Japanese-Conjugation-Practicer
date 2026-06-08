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
}

public enum AdjectiveType
{
    I, //i-adjective
    NA, // na-adjective
    IRR, // irregular
}

public enum AdjectiveConjugation
{
    StandardNonpast,
    PoliteNonpast,
    PoliteNonpastNegative,
    PolitePast,
    PolitePastNegative,
    StandardNonpastNegative,
    StandardPast,
    StandardPastNegative,
}
