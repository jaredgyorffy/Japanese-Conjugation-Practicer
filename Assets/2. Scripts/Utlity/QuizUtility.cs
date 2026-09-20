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


    public static bool CheckAnswer(string userAnswer, Question question)
    {
        if (question.Category == QuestionCategory.Expression || question.Category == QuestionCategory.Vocab)
        {
            foreach (string answer in question.Answers)
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
            foreach (string answer in question.Answers)
            {
                if (userAnswer == answer)
                {
                    return true;
                }
                else if (userAnswer == StringUtility.KatakanaToHiragana(answer))
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

    public static Question GetExpressionQuestion(IWord word)
    {
        string questionType = "Expression Meaning";
        string hint = "No hints currently available for Expressions";
        return new Question(questionType, word.Meaning, word, QuestionCategory.Expression, hint);
    }

    public static Question GetNumbersQuestion(int number)
    {
        string questionType = "";
        string hint = "No hints currently available for Numbers";
        if (RandomUtility.PercentageChanceOfTrue(0.5f))
        {
            questionType = "Write the number";
            Noun word = new();
            word.kanji = NumberTranslator.GetNumberTranslation(number)[0];
            List <string> correctAnswers = new List<string>();
            correctAnswers.Add(number.ToString());
            return new Question(questionType, correctAnswers, word, QuestionCategory.Numbers, hint);
        }
        else
        {
            questionType = "Translate the number";
            Noun word = new();
            word.kanji = number.ToString();
            return new Question(questionType, NumberTranslator.GetNumberTranslation(number), word, QuestionCategory.Numbers, hint);
        }

    }

    public static Question GetVocabQuestion(ref WordLists wordlist)
    {
        string questionType = "Meaning";
        string hint = "No hints are available for meaning questions";

        IWord word = GetRandomWord(ref wordlist, GetRandomWordType(wordlist));
        return new Question(questionType, word.Meaning, word, QuestionCategory.Vocab, hint);
    }


    public static Question GetConjugationQuestion(WordType wordType, ConjugationType form, IWord word)
    {
        string questionType = "";
        List<string> answer = new();
        string hint = Hint.GetHint(word, form);

        if (wordType == WordType.Verb)
        {
            Verb Word = word as Verb;

            switch (form)
            {
            case ConjugationType.PoliteNonpast:
                answer.Add(Word.PoliteNonpast);
                questionType = "Polite Non-past Form";
                break;
            case ConjugationType.PoliteNonpastNegative:
                answer.Add(Word.PoliteNonPastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(Word.PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(Word.PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(Word.StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpast:
                answer.Add(Word.StandardNonpast);
                questionType = "Type the Hiragana";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(Word.StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(Word.StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.PoliteVolitional:
                answer.Add(Word.PoliteVolitional);
                questionType = "Polite Volitional Form";
                break;
            case ConjugationType.TeForm:
                answer.Add(Word.TeForm);
                questionType = "Te-form";
                break;
            case ConjugationType.CasualVolitional:
                answer.Add(Word.CasualVolitional);
                questionType = "Casual Volitional Form";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }
        else if (wordType == WordType.Adjective)
        {
            Adjective Word = word as Adjective;
            switch (form)
            {

            case ConjugationType.PoliteNonpastNegative:
                answer.Add(Word.PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(Word.PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(Word.PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(Word.StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(Word.StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(Word.StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.StandardNonpast:
                answer.Add(Word.StandardNonpast);
                questionType = "Type the Hiragana";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }
        else if (wordType == WordType.Noun)
        {
            Noun Word = word as Noun;
            switch (form)
            {
            case ConjugationType.PoliteNonpastNegative:
                answer.Add(Word.PoliteNonpastNegative);
                questionType = "Polite Non-past Negative Form";
                break;
            case ConjugationType.PolitePast:
                answer.Add(Word.PolitePast);
                questionType = "Polite Past Form";
                break;
            case ConjugationType.PolitePastNegative:
                answer.Add(Word.PolitePastNegative);
                questionType = "Polite Past Negative Form";
                break;
            case ConjugationType.StandardPast:
                answer.Add(Word.StandardPast);
                questionType = "Standard Past Form";
                break;
            case ConjugationType.StandardNonpastNegative:
                answer.Add(Word.StandardNonpastNegative);
                questionType = "Standard Non-past Negative Form";
                break;
            case ConjugationType.StandardPastNegative:
                answer.Add(Word.StandardPastNegative);
                questionType = "Standard Past Negative Form";
                break;
            case ConjugationType.StandardNonpast:
                answer.Add(Word.StandardNonpast);
                questionType = "Type the Hiragana";
                break;
            default:
                Debug.LogWarning("Error: Question Type not valid");
                break;
            }
        }

        return new Question(questionType, answer, word, QuestionCategory.Conjugation, hint);
    }
    public static WordType GetRandomWordType(ConjugationTypes words, WordLists lists)
    {
        WordType wordtype;
        bool containsVerb = words.VerbConjugationTypes.Count > 0 && lists.Verbs.Count > 0;
        bool containsAdjective = words.AdjectiveConjugationTypes.Count > 0 && lists.Adjectives.Count > 0;
        bool containsNoun = words.NounConjugationTypes.Count > 0 && lists.Nouns.Count > 0;
        if (containsVerb && containsAdjective == false && containsNoun == false)
        {
            wordtype = WordType.Verb;
        }
        else if (containsVerb == false && containsAdjective && containsNoun == false)
        {
            wordtype = WordType.Adjective;
        }
        else if (containsVerb == false && containsAdjective == false && containsNoun)
        {
            wordtype = WordType.Noun;
        }
        else if (containsVerb && containsAdjective && containsNoun == false)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Verb : WordType.Adjective;
        }
        else if (containsVerb == false && containsAdjective && containsNoun)
        {
            wordtype = RandomUtility.PercentageChanceOfTrue(0.5f) ? WordType.Noun : WordType.Adjective;
        }
        else if (containsVerb && containsAdjective == false && containsNoun)
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

    public static WordType GetRandomWordType(WordLists words)
    {
        List<WordType> wordTypes = new();
        if (words.Adjectives.Count > 0)
        {
            wordTypes.Add(WordType.Adjective);
        }

        if (words.Nouns.Count > 0)
        {
            wordTypes.Add(WordType.Noun);
        }

        if (words.Verbs.Count > 0)
        {
            wordTypes.Add(WordType.Verb);
        }

        if (words.Adverbs.Count > 0)
        {
            wordTypes.Add(WordType.Adverb);
        }



        return wordTypes[UnityEngine.Random.Range(0, wordTypes.Count)];
    }

    public static Question GetRandomGrammer(WordLists words)
    {
        Grammer grammer = words.Grammers[UnityEngine.Random.Range(0, words.Grammers.Count)];
        GrammerExample example = grammer.Examples[UnityEngine.Random.Range(0, grammer.Examples.Count)];
        List<string> answers = new List<string>();
        answers.Add(example.Answer);
        Noun fakeWord = new Noun(example.Question, "", answers);

        return new Question(example.Hint, answers, fakeWord, QuestionCategory.Grammer, example.Translation);
    }

    public static IWord GetRandomWord(ref WordLists words, WordType wordtype)
    {
        IWord word = null;
        int index = 0;
        if (wordtype == WordType.Verb && words.Verbs.Count > 0)
        {
            index = UnityEngine.Random.Range(0, words.Verbs.Count);

            word = words.Verbs[index];
            words.Verbs.RemoveAt(index);
        }
        else if (wordtype == WordType.Adjective && words.Adjectives.Count > 0)
        {
            index = UnityEngine.Random.Range(0, words.Adjectives.Count);

            word = words.Adjectives[index];
            words.Adjectives.RemoveAt(index);
        }
        else if (wordtype == WordType.Noun && words.Nouns.Count > 0)
        {
            index = UnityEngine.Random.Range(0, words.Nouns.Count);

            word = words.Nouns[index];
            words.Nouns.RemoveAt(index);
        }
        else if (wordtype == WordType.Expression && words.Expressions.Count > 0)
        {
            index = UnityEngine.Random.Range(0, words.Expressions.Count);

            word = words.Expressions[index];
            words.Expressions.RemoveAt(index);
        }
        else if (wordtype == WordType.Adverb && words.Adverbs.Count > 0)
        {
            index = UnityEngine.Random.Range(0, words.Adverbs.Count);

            word = words.Adverbs[index];
            words.Adverbs.RemoveAt(index);
        }

        return word;
    }
}
