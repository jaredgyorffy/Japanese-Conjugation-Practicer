using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Expression : IWord
{
    public string Kana => kana;
    [SerializeField] private string kana;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public WordType WordType => WordType.Expression;
    public string MeaningFull => meaningFull;
    private string meaningFull;
    public List<string> Meaning => meaning;
    [SerializeField] private List<string> meaning;
}