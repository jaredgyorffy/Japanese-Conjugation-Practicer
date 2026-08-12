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
}
