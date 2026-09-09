using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class VocabSpreadsheet
{
    [SpreadsheetPage("verbs")]
    public List<Verb> verbs;
    [SpreadsheetPage("nouns")]
    public List<Noun> nouns;
    [SpreadsheetPage("adjectives")]
    public List<Adjective> adjectives;
    [SpreadsheetPage("expressions")]
    public List<Expression> expressions;
    [SpreadsheetPage("adverbs")]
    public List<Adverb> adverbs;
}
[CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpreadsheetContainer")]
public class SpreadsheetContainer : SpreadsheetsContainerBase
{
    [SpreadsheetContent]
    [SerializeField] VocabSpreadsheet content;
    public VocabSpreadsheet Content => content;
}