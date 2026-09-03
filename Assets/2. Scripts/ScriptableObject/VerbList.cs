using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "WordList", menuName = "Scriptable Objects/VerbList")]
public class VerbList : ScriptableObject
{
    [Button]
    public void AddAnswersNouns()
    {
        foreach (Noun noun in nounList)
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
        foreach (Verb verb in verbList)
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
        foreach (Adjective verb in adjectiveList)
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
    public void RemoveDuplicates()
    {
        foreach (Noun noun in nounList)
        {
            HashSet<string> noDuplicates = new HashSet<string>(
                    noun.Meaning,
                    System.StringComparer.OrdinalIgnoreCase
                );
            noun.Meaning.Clear();
            noun.Meaning.AddRange(new List<string>(noDuplicates).ToArray());
        }
    }
    [field: SerializeField] public string listName { get; private set; }
    [field: SerializeField][FormerlySerializedAs("List")] public List<Verb> verbList;
    [field: SerializeField] public List<Adjective> adjectiveList;
    [field: SerializeField] public List<Noun> nounList;
}