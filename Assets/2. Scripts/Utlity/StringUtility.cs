using System.Linq;
using UnityEngine;

public static class StringUtility
{
    public static bool ContainsEnglishCharacters(this string text)
    {
        bool isEnglishCharacter = text.Any(c =>
        (c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z'));

        bool isNumber = text.Any(char.IsDigit);
        return isEnglishCharacter || isNumber;
    }

    public static string KatakanaToHiragana(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        char[] chars = text.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            // Standard katakana range: ァ (30A1) through ヶ (30F6)
            if (chars[i] >= '\u30A1' && chars[i] <= '\u30F6')
            {
                // Katakana and hiragana are offset by 0x60
                chars[i] = (char)(chars[i] - 0x60);
            }
        }

        return new string(chars);
    }
}
