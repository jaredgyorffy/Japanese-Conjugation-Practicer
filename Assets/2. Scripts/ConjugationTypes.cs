using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
public class ConjugationTypes
{
    public ConjugationTypes(List<ConjugationType> verbs, List<ConjugationType> adjectives, List<ConjugationType> nouns)
    {
        VerbConjugationTypes = verbs;
        AdjectiveConjugationTypes = adjectives;
        NounConjugationTypes = nouns;
    }

    public List<ConjugationType> VerbConjugationTypes;
    public List<ConjugationType> AdjectiveConjugationTypes;
    public List<ConjugationType> NounConjugationTypes;
}
