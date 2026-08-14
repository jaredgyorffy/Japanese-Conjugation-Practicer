using UnityEngine;
using System.Collections.Generic;

public class WordLists
{
    public WordLists(List<Verb> Verbs, List<Adjective> Adjectives, List<Noun> Nouns)
    {
        this.Verbs = Verbs;
        this.Adjectives = Adjectives;
        this.Nouns = Nouns;
    }

    public List<Verb> Verbs;
    public List<Adjective> Adjectives;
    public List<Noun> Nouns;
}
