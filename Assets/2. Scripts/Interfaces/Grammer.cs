using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Grammer
{
    public Grammer(string kana, string kanji, string Key)
    {
        this.kana = kana;
        this.kanji = kanji;
        this.Key = Key;
    }
    public string Kana => kana;
    [SerializeField] private string kana;

    public string Kanji => kanji;
    [SerializeField] public string kanji;
    [field: SerializeField] public string Key;
    [field: SerializeField] public List<GrammerExample> Examples;
    public string Meaning => meaning;
    [SerializeField] private string meaning;
}
[Serializable]
public class GrammerExample
{
    public string Question;
    public string Answer;
    public string Key { get; set; }
    public string Translation;
    public string Hint;
}