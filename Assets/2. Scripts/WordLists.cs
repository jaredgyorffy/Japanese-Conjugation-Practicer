using UnityEngine;
using System.Collections.Generic;

public class WordLists
{
    public WordLists(List<Verb> Verbs, List<Adjective> Adjectives, 
        List<Noun> Nouns, List<Expression> expressions, List<Adverb> adverbs, List<Grammer> grammers)
    {
        this.Verbs = Verbs;
        this.Adjectives = Adjectives;
        this.Nouns = Nouns;
        this.Expressions = expressions;
        this.Adverbs = adverbs;
        this.Grammers = grammers;
    }

    public List<Grammer> Grammers;
    public List<Verb> Verbs;
    public List<Adjective> Adjectives;
    public List<Noun> Nouns;
    public List<Adverb> Adverbs;
    public List<Expression> Expressions;
}
