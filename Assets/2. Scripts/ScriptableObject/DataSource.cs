using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DataSource", menuName = "ScriptableObject/DataSource")]
public class DataSource : ScriptableObject
{
    [Button]
    public void AddAnswersNouns()
    {
        foreach (Noun noun in NounList)
        {
            List<string> items = noun.MeaningFull
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
            noun.Meaning.AddRange(items);
        }
    }

    [Button]
    public void AddAnswersVerbs()
    {
        foreach (Verb verb in VerbList)
        {
            List<string> items = verb.MeaningFull
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
            verb.Meaning.AddRange(items);
        }
    }

    [Button]
    public void AddAnswersAdjectives()
    {
        foreach (Adjective verb in AdjectiveList)
        {
            List<string> items = verb.MeaningFull
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
            verb.Meaning.AddRange(items);
        }
    }

    [Button]
    public void AddAnswersExpressions()
    {
        foreach (Expression expression in ExpressionList)
        {
            List<string> items = expression.MeaningFull
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
            expression.Meaning.AddRange(items);
        }
    }

    [Button]
    public void AddAnswersAdverbs()
    {
        foreach (Adverb adverb in AdverbList)
        {
            List<string> items = adverb.MeaningFull
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
            adverb.Meaning.AddRange(items);
        }
    }

    [Button]
    public void RemoveDuplicates()
    {
        foreach (Noun noun in NounList)
        {
            HashSet<string> noDuplicates = new HashSet<string>(
                    noun.Meaning,
                    System.StringComparer.OrdinalIgnoreCase
                );
            noun.Meaning.Clear();
            noun.Meaning.AddRange(new List<string>(noDuplicates).ToArray());
        }
    }
    [field: SerializeField] public string SourceName { get; private set; }
    [field: SerializeField] public List<QuestionType> QuestionTypes;
    [field: SerializeField] public List<Verb> VerbList;
    [field: SerializeField] public List<Adverb> AdverbList;
    [field: SerializeField] public List<Adjective> AdjectiveList;
    [field: SerializeField] public List<Noun> NounList;
    [field: SerializeField] public List<Expression> ExpressionList;
}