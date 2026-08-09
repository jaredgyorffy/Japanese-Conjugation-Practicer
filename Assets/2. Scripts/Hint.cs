
using System;

public static class Hint
{
    public static string GetHint(IWord word, ConjugationType conjugationType)
    {
        string hint = "";
        
        if (word.WordType == WordType.Verb)
        {
            VerbType vType = ((Verb)word).VerbType;
            return VerbHint(vType, conjugationType);
        }
        
        if (word.WordType == WordType.Adjective)
        {
            AdjectiveType aType = ((Adjective)word).AdjectiveType;
            return AdjectiveHint(aType, conjugationType);
        }

        return hint;
    }

    private static string AdjectiveHint(AdjectiveType aType, ConjugationType conjugationType)
    {
       string hint = "";
        switch (conjugationType)
        {
            case ConjugationType.PoliteNonpast:
            {
                if (aType == AdjectiveType.I)
                {
                    hint = "No additional conjugation is needed to derive the I-Adjective Polite non-past form from the dictionary form. When used at the end of a sentence, use the pattern [Adjective + です].  When used to describe a noun, use the pattern [Adjective + Noun]";
                }
                else if (aType == AdjectiveType.NA)
                {
                    hint = "No additional conjugation is needed to derive the Na-Adjective Polite non-past form from the dictionary form. When used at the end of a sentence, use the pattern [Adjective + です]. When used to describe a noun, use the pattern [Adjective + な + Noun]";
                }
                else
                {
                    hint = "No additional conjugation is needed to derive the Irregular Polite non-past form from the dictionary form. When used at the end of a sentence, use the pattern [Adjective + です].  When used to describe a noun, use the pattern [Adjective + Noun]";
                }
                break;
            }
            case  ConjugationType.PoliteNonpastNegative:
            {
                if (aType == AdjectiveType.I)
                {
                    hint = "To conjugate the I-adjective Polite Nonpast Negative form, replace い with くありません.";
                }
                else if (aType == AdjectiveType.NA)
                {
                    hint = "To conjugate the NA-adjective Polite Nonpast Negative form, Add じゃありません.";
                }
                else
                {
                    hint = "いい conjugates to よ. Then we apply the polite nonpast negative conjugation rules: add くない.";
                }
                break;
            }
            case ConjugationType.PolitePast:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "PolitePast";
                break;
            }

            case ConjugationType.PolitePastNegative:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "PolitePastNegative";
                break;
            }
            case ConjugationType.PoliteVolitional:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "PoliteVolitional";
                break;
            }

            case ConjugationType.StandardNonpast:
            {
                if (aType == AdjectiveType.I)
                {
                    hint = "No additional conjugation is needed to derive the I-Adjective Standard non-past form from the dictionary form. Never use だ with i-Adjectives. When used to describe a noun, use the pattern [Adjective + Noun]";
                }
                else if (aType == AdjectiveType.NA)
                {
                    hint = "No additional conjugation is needed to derive the Na-Adjective standard non-past form from the dictionary form. When used at the end of a sentence, use the pattern [Adjective + だ]. When used to describe a noun, use the pattern [Adjective + な + Noun]";
                }
                else
                {
            
                }
                hint = "StandardNonpast";
                break;
            }
            case ConjugationType.StandardNonpastNegative:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "StandardNonpastNegative";
                break;
            }
            case ConjugationType.StandardPast:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "StandardPast";
                break;
            }
            case ConjugationType.StandardPastNegative:
            {
                if (aType == AdjectiveType.I)
                {
            
                }
                else if (aType == AdjectiveType.NA)
                {
            
                }
                else
                {
            
                }
                hint = "StandardPastNegative";
                break;
            }
            
            default:
            {
                hint = "conjugationType Hint not Supported";
                throw new Exception("conjugationType Hint not Supported");
            }
        }

        return hint;
    }

    private static string VerbHint(VerbType vType, ConjugationType conjugationType)
    {
        string hint = "";
        switch (conjugationType)
        {
            case ConjugationType.PoliteNonpast:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form, then add ます.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-Verb: Remove the う hiragana from the end of the plain form, then add ます.";
                }
                else
                {
                    hint = "Irregular: The stem of する conjugates to し and the stem of くる conjugates to き. Then add ます";
                }
                break;
            }
            case  ConjugationType.PoliteNonpastNegative:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form, then add ません.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-Verb: Remove the う hiragana from the end of the plain form, then add ません.";
                }
                else
                {
                    hint = "Irregular: The stem of する conjugates to し and the stem of くる conjugates to き. Then add ません";
                }
                break;
            }
            case ConjugationType.PolitePast:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form, then add ました.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-Verb: Remove the う hiragana from the end of the plain form, then add ました.";
                }
                else
                {
                    hint = "Irregular: The stem of する conjugates to し and the stem of くる conjugates to き. Then add ました";
                }
                break;
            }

            case ConjugationType.PolitePastNegative:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form, then add ませんでした.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-Verb: Remove the う hiragana from the end of the plain form, then add ませんでした.";
                }
                else
                {
                    hint = "Irregular：　The stem of する conjugates to し and the stem of くる conjugates to き. Then add ませんでした";
                }
                break;
            }
            case ConjugationType.PoliteVolitional:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form to get the verb stem, then add ましょう.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-Verb: replace the /う/ hiragana from the end of the plain form with the appropriate /い/ column hiragana:" +
                        "'う':いましょう\r\n" +
                        "'く':きましょう\r\n" +
                        "'す':しましょう\r\n" +
                        "'つ':ちましょう\r\n" +
                        "'る':りましょう\r\n" +
                        "'ぬ':にましょう\r\n" +
                        "'む':みましょう\r\n" +
                        "'ぶ':びましょう\r\n" +
                        "'ぐ':ぎましょう\r\n";
                }
                else
                {
                    hint = "Irregular: The stem of する conjugates to し and the stem of くる conjugates to き. Then add ましょう";
                }
                break;
            }

            case ConjugationType.StandardNonpast:
            {
                if (vType == VerbType.RU)
                {
                    hint = "No conjugation needed here, standard nonpast is the same as the plain (dictionary) form";
                }
                else if (vType == VerbType.U)
                {
                    hint = "No conjugation needed here, standard nonpast is the same as the plain (dictionary) form";
                }
                else
                {
                    hint = "No conjugation needed here, standard nonpast is the same as the plain (dictionary) form";
                }
                break;
            }
            case ConjugationType.StandardNonpastNegative:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form to get the verb stem, then add ない.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "Ru-verb: Replace the /う/ column with it's /あ/ column equivilient (ex. く becomes か). Then add ない.";
                }
                else
                {
                    hint = "Although ある is not irregular, in the plain negative form, ない simply replaces ある altogether. Otherwise: " +
                        "\n する　＞　しない" +
                        "\n 来る ＞ こない";
                }
                break;
            }
            case ConjugationType.StandardPast:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form to get the verb stem, then add た.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "U-verb: Replace the /う/ from the end of the plain form according to the following rules: " +
                        "［る］［う］［つ］Verb ￫ った \n" +
                        "［く］［ぐ］￫ いだ \n" +
                        "［ぬ］［ぶ］［む］￫ んだ \n" +
                        "［す］￫ 話す + した";
                }
                else
                {
                    hint =
                        "行く ￫ 行った\n" +
                        "する ￫ した\n" +
                        "くる ￫ きた\n" +
                        "問う ￫ 問うた\n" +
                        "請う ￫ 請うた";
                }
                break;
            }
            case ConjugationType.StandardPastNegative:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form to get the verb stem, then add なかった.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "Convert the う sound into it's あ column equivilent, then add なかった \r\n" +
                        "る ￫ らなかった\r\n" +
                        "う ￫ わなかった\r\n" +
                        "く ￫ かなかった\r\n" +
                        "す ￫ さなかった\r\n" +
                        "つ ￫ たなかった\r\n" +
                        "ぬ ￫ ななかった\r\n" +
                        "ぶ ￫ ばなかった\r\n" +
                        "む ￫ まなかった\r\n" +
                        "ぐ ￫ がなかった\r\n. ";
                }
                else
                {
                    hint = "Exceptions: ある ￫ なかった";
                }
                break;
            }
            case ConjugationType.TeForm:
            {
                if (vType == VerbType.RU)
                {
                    hint = "Ru-verb: Removing the る from the end of the plain form to get the verb stem, then add て.";
                }
                else if (vType == VerbType.U)
                {
                    hint = "Ru-verb: replacing the /う/ from the end of the plain form according to the following rules:" +
                        "\r\n［る］［う］［つ］￫ って\r\n" +
                        "［く］［ぐ］￫ いて\r\n" +
                        "［ぬ］［ぶ］［む］￫ んで\r\n" +
                        "［す］￫ して ";
                }
                else
                {
                    hint = "行く ￫ 行って\r\n" +
                        "する ￫ して\r\n" +
                        "くる ￫ きて\r\n" +
                        "問う ￫ 問うて\r\n" +
                        "請う ￫ 請うて";
                }
                hint = "TeForm";
                break;
            }
            case ConjugationType.CasualVolitional:
            {
                if (vType == VerbType.RU)
                {

                }
                else if (vType == VerbType.U)
                {
            
                }
                else
                {
            
                }
                hint = "CasualVolitional";
                break;
            }
            
            default:
            {
                hint = "conjugationType Hint not Supported";
                throw new Exception("conjugationType Hint not Supported");
            }
        }

        return hint;
    }
}
