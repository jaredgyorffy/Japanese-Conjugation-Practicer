using System;
using System.Collections.Generic;
using Unity.Properties;
using Hieki.Search;
using UnityEngine;
using UnityEngine.UIElements;
using NaughtyAttributes;

public static class QuizUtility
{
    public static ConjugationTypes InitializeQuestionTypes(QuizConfiguration config)
    {
        List<ConjugationType> verbConjugationTypes = new List<ConjugationType>();
        if (config.VerbPoliteNonpastForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteNonpast);
        }
        if (config.VerbPoliteNonpastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.VerbPolitePastForm)
        {
            verbConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.VerbPolitePastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.VerbStandardPastForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.VerbStandardNonpastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.VerbStandardPastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.VerbPoliteVolitionalForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteVolitional);
        }
        if (config.VerbTeForm)
        {
            verbConjugationTypes.Add(ConjugationType.TeForm);
        }
        if (config.VerbMeaning)
        {
            verbConjugationTypes.Add(ConjugationType.Meaning);
        }

        List<ConjugationType> adjectiveConjugationTypes = new List<ConjugationType>();
        if (config.AdjectivePoliteNonpastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.AdjectivePolitePastForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.AdjectivePolitePastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.AdjectiveStandardPastForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.AdjectiveStandardNonpastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.AdjectiveStandardPastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.AdjectiveMeaning)
        {
            adjectiveConjugationTypes.Add(ConjugationType.Meaning);
        }

        List<ConjugationType> nounConjugationTypes = new List<ConjugationType>();
        if (config.NounPoliteNonpastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.NounPolitePastForm)
        {
            nounConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.NounPolitePastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.NounStandardPastForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.NounStandardNonpastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.NounStandardPastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.NounMeaning)
        {
            nounConjugationTypes.Add(ConjugationType.Meaning);
        }
        return new ConjugationTypes(verbConjugationTypes, adjectiveConjugationTypes, nounConjugationTypes);
    }

    public static ConjugationTypes GenerateRandomConjugationTypes(QuizConfiguration config, int conjugationTypes)
    {
        List<ConjugationType> verbConjugationTypes = new List<ConjugationType>();
        if (config.VerbPoliteNonpastForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteNonpast);
        }
        if (config.VerbPoliteNonpastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.VerbPolitePastForm)
        {
            verbConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.VerbPolitePastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.VerbStandardPastForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.VerbStandardNonpastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.VerbStandardPastNegativeForm)
        {
            verbConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.VerbPoliteVolitionalForm)
        {
            verbConjugationTypes.Add(ConjugationType.PoliteVolitional);
        }
        if (config.VerbTeForm)
        {
            verbConjugationTypes.Add(ConjugationType.TeForm);
        }
        if (config.VerbMeaning)
        {
            verbConjugationTypes.Add(ConjugationType.Meaning);
        }

        List<ConjugationType> adjectiveConjugationTypes = new List<ConjugationType>();
        if (config.AdjectivePoliteNonpastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.AdjectivePolitePastForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.AdjectivePolitePastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.AdjectiveStandardPastForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.AdjectiveStandardNonpastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.AdjectiveStandardPastNegativeForm)
        {
            adjectiveConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.AdjectiveMeaning)
        {
            adjectiveConjugationTypes.Add(ConjugationType.Meaning);
        }

        List<ConjugationType> nounConjugationTypes = new List<ConjugationType>();
        if (config.NounPoliteNonpastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.PoliteNonpastNegative);
        }
        if (config.NounPolitePastForm)
        {
            nounConjugationTypes.Add(ConjugationType.PolitePast);
        }
        if (config.NounPolitePastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.PolitePastNegative);
        }
        if (config.NounStandardPastForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardPast);
        }
        if (config.NounStandardNonpastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardNonpastNegative);
        }
        if (config.NounStandardPastNegativeForm)
        {
            nounConjugationTypes.Add(ConjugationType.StandardPastNegative);
        }
        if (config.NounMeaning)
        {
            nounConjugationTypes.Add(ConjugationType.Meaning);
        }
        return new ConjugationTypes(verbConjugationTypes, adjectiveConjugationTypes, nounConjugationTypes);
    }

    
    public static ConjugationType GetRandomQuestionType(WordType wordtype, ConjugationTypes conjugationTypes)
    {
        if (wordtype == WordType.Verb)
        {
            int index = UnityEngine.Random.Range(0, conjugationTypes.VerbConjugationTypes.Count);
            return conjugationTypes.VerbConjugationTypes[index];
        }
        else if (wordtype == WordType.Adjective)
        {
            int index = UnityEngine.Random.Range(0, conjugationTypes.AdjectiveConjugationTypes.Count);
            return conjugationTypes.AdjectiveConjugationTypes[index];
        }
        else if (wordtype == WordType.Noun)
        {
            int index = UnityEngine.Random.Range(0, conjugationTypes.NounConjugationTypes.Count);
            return conjugationTypes.NounConjugationTypes[index];
        }
        else
        {
            Debug.LogWarning("Unable to select question type because invalid wordtype was supplied");
            return 0;
        }
    }


    public static bool CheckAnswer(string userAnswer, List<string> correctAnswers, ConjugationType currentConjugationType)
    {
        if (currentConjugationType == ConjugationType.Meaning)
        {
            foreach (string answer in correctAnswers)
            {
                double score = Fuzzy.Ratio(userAnswer, answer);
                if (score > 0.85)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (string answer in correctAnswers)
            {
                if (userAnswer == answer)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static string GetAnswer(List<string> correctAnswers)
    {
            return correctAnswers[0];
    }


    public static (List<string> answer, string questionType) GetQuestionAndAnswer(Question question, WordLists wordlist)
    {
        int wordIndex = question.Index;
        ConjugationType form = question.conjugationType;
        WordType wordType = question.Wordtype;
        string questionType = "";
        List<string> answer = new();

        if (wordType == WordType.Verb)
        {
            switch (form)
            {
            case ConjugationType.PoliteNonpast:
                answer.Add(wordlist.Verbs[wordIndex].PoliteNonpast);
                questionType = "Polite Non-past Form";
                break;
            case ConjugationType.PoliteNonpastNegative:
                answer.Add(wordlist.Verbs[wordIndex].PoliteNonPastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(wordlist.Verbs[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(wordlist.Verbs[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(wordlist.Verbs[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpast:
                answer.Add(wordlist.Verbs[wordIndex].StandardNonpast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(wordlist.Verbs[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(wordlist.Verbs[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.PoliteVolitional:
                answer.Add(wordlist.Verbs[wordIndex].PoliteVolitional);
                questionType = "Polite Volitional Form";
                break;
            case ConjugationType.TeForm:
                answer.Add(wordlist.Verbs[wordIndex].TeForm);
                questionType = "Te-form";
                break;
            case ConjugationType.CasualVolitional:
                answer.Add(wordlist.Verbs[wordIndex].CasualVolitional);
                questionType = "Casual Volitional Form";
                break;
            case ConjugationType.Meaning:
                answer = (wordlist.Verbs[wordIndex].Meaning);
                questionType = "Meaning";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }
        else if (wordType == WordType.Adjective)
        {
            switch (form)
            {

            case ConjugationType.PoliteNonpastNegative:
                answer.Add(wordlist.Adjectives[wordIndex].PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(wordlist.Adjectives[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(wordlist.Adjectives[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(wordlist.Adjectives[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(wordlist.Adjectives[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(wordlist.Adjectives[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.Meaning:
                answer = (wordlist.Adjectives[wordIndex].Meaning);
                questionType = "Meaning";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }
        else if (wordType == WordType.Noun)
        {
            switch (form)
            {
            case ConjugationType.PoliteNonpastNegative:
                answer.Add(wordlist.Nouns[wordIndex].PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(wordlist.Nouns[wordIndex].PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(wordlist.Nouns[wordIndex].PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(wordlist.Nouns[wordIndex].StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(wordlist.Nouns[wordIndex].StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(wordlist.Nouns[wordIndex].StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.Meaning:
                answer = (wordlist.Nouns[wordIndex].Meaning);
                questionType = "Meaning";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }

        return (answer, questionType);
    }
    public static WordType GetRandomWordType(ConjugationTypes words)
    {
        WordType wordtype;
        if (words.VerbConjugationTypes.Count > 0 && words.AdjectiveConjugationTypes.Count == 0 && words.NounConjugationTypes.Count == 0)
        {
            wordtype = WordType.Verb;
        }
        else if (words.VerbConjugationTypes.Count == 0 && words.AdjectiveConjugationTypes.Count > 0 && words.NounConjugationTypes.Count == 0)
        {
            wordtype = WordType.Adjective;
        }
        else if (words.VerbConjugationTypes.Count == 0 && words.AdjectiveConjugationTypes.Count == 0 && words.NounConjugationTypes.Count > 0)
        {
            wordtype = WordType.Noun;
        }
        else if (words.VerbConjugationTypes.Count > 0 && words.AdjectiveConjugationTypes.Count > 0 && words.NounConjugationTypes.Count == 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Verb : WordType.Adjective;
        }
        else if (words.VerbConjugationTypes.Count == 0 && words.AdjectiveConjugationTypes.Count > 0 && words.NounConjugationTypes.Count > 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Noun : WordType.Adjective;
        }
        else if (words.VerbConjugationTypes.Count > 0 && words.AdjectiveConjugationTypes.Count == 0 && words.NounConjugationTypes.Count > 0)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Noun : WordType.Verb;
        }
        else
        {
            int random = UnityEngine.Random.Range(0, 3);
            if (random == 0)
            {
                wordtype = WordType.Noun;
            }
            else if (random == 1)
            {
                wordtype = WordType.Verb;
            }
            else
            {
                wordtype = WordType.Adjective;
            }
        }

        return wordtype;
    }

    public static int GetRandomWordIndex(WordLists words, WordType wordtype)
    {
        int wordIndex = 0;
        if (wordtype == WordType.Verb)
        {
            wordIndex = UnityEngine.Random.Range(0, words.Verbs.Count);
        }
        else if (wordtype == WordType.Adjective)
        {
            wordIndex = UnityEngine.Random.Range(0, words.Adjectives.Count);
        }
        else if (wordtype == WordType.Noun)
        {
            wordIndex = UnityEngine.Random.Range(0, words.Nouns.Count);
        }

        return wordIndex;
    }
}
