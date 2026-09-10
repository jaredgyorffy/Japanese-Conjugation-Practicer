using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
[Serializable]
public class VocabData
{
    [SpreadsheetPage("verbs")]
    public List<Verb> VerbList;
    [SpreadsheetPage("nouns")]
    public List<Noun> NounList;
    [SpreadsheetPage("adjectives")]
    public List<Adjective> AdjectiveList;
    [SpreadsheetPage("expressions")]
    public List<Expression> ExpressionList;
    [SpreadsheetPage("adverbs")]
    public List<Adverb> AdverbList;

    public List<Grammer> GrammerList;
    [SpreadsheetPage("grammer")]
    public List<GrammerExample> GrammerExamples;
}

[CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpreadsheetContainer")]
public class SpreadsheetContainer : SpreadsheetsContainerBase
{
    [SpreadsheetContent]
    [SerializeField] VocabData content;
    public VocabData Content => content;
}