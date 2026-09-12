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
    public void FormatData()
    {
        AddAnswersNouns();
        AddAnswersVerbs();
        AddAnswersAdjectives();
        AddAnswersAdverbs();
        AddAnswersExpressions();
    }

    public void AddAnswersNouns()
    {
        foreach (Noun noun in VocabData.NounList)
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

    public void AddAnswersVerbs()
    {
        foreach (Verb verb in VocabData.VerbList)
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

    public void AddAnswersAdjectives()
    {
        foreach (Adjective verb in VocabData.AdjectiveList)
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

    public void AddAnswersExpressions()
    {
        foreach (Expression expression in VocabData.ExpressionList)
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

    public void AddAnswersAdverbs()
    {
        foreach (Adverb adverb in VocabData.AdverbList)
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
    public void PopulateGrammerExamples()
    {
        foreach (GrammerExample example in VocabData.GrammerExamples)
        {
            bool containsKey = false;

            foreach (var grammer in VocabData.GrammerList)
            {
                if (grammer.Key == example.Key)
                {
                    grammer.Examples.Add(example);
                    containsKey = true;
                }
            }

            if (containsKey)
            {
                continue;
            }
            Grammer newGrammer = new Grammer(example.Answer, example.Answer, example.Key);
            newGrammer.Examples = new();
            newGrammer.Examples.Add(example);
            VocabData.GrammerList.Add(newGrammer);
        }
    }

    public void RemoveDuplicates()
    {
        foreach (Noun noun in VocabData.NounList)
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
    [field: SerializeField] public VocabData VocabData;
}