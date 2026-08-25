using UnityEngine;
using System.Collections.Generic;
using System;
using static ListUtility;
[Serializable]
public class ConjugationTypes
{
    public ConjugationTypes(List<ConjugationType> verbs, List<ConjugationType> adjectives, List<ConjugationType> nouns)
    {
        VerbConjugationTypes = verbs;
        AdjectiveConjugationTypes = adjectives;
        NounConjugationTypes = nouns;

        VerbConjugationTypes.Shuffle();
        AdjectiveConjugationTypes.Shuffle();
        NounConjugationTypes.Shuffle();
        verbIndex = 0;
        adjectiveIndex = 0;
        nounIndex = 0;
    }

    private int verbIndex;
    private int adjectiveIndex;
    private int nounIndex;

    public ConjugationType GetConjugationTypeByWordType(WordType wordType)
    {
        switch (wordType)
        {
        case WordType.Noun:
            return GetNextNounConjugationType();
        case WordType.Adjective:
            return GetNextAdjectiveConjugationType();
        case WordType.Verb:
            return GetNextVerbConjugationType();
        default:
            return GetNextVerbConjugationType();
        }
    }

    public ConjugationType GetNextVerbConjugationType()
    {
        return GetConjugationType(VerbConjugationTypes, ref verbIndex);
    }

    public ConjugationType GetNextNounConjugationType()
    {
        return GetConjugationType(NounConjugationTypes, ref nounIndex);
    }

    public ConjugationType GetNextAdjectiveConjugationType()
    {
        return GetConjugationType(AdjectiveConjugationTypes, ref adjectiveIndex);
    }

    private ConjugationType GetConjugationType(List<ConjugationType> conjugations, ref int index)
    {
        ConjugationType typeToReturn = conjugations[index];
        index += 1;
        if (index >= conjugations.Count)
        {
            index = 0;
            conjugations.Shuffle();
        }
        return typeToReturn;
    }

    public List<ConjugationType> VerbConjugationTypes;
    public List<ConjugationType> AdjectiveConjugationTypes;
    public List<ConjugationType> NounConjugationTypes;
}
