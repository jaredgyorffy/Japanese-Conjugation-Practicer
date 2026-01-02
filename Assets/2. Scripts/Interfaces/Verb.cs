using UnityEngine;

[CreateAssetMenu(fileName = "Verb", menuName = "Scriptable Objects/Verb")]
public class Verb : ScriptableObject, IWord
{
    public WordType WordType => wordtype;
    [SerializeField] private WordType wordtype;

    public VerbType VerbType => verbType;
    [SerializeField] private VerbType verbType;

    public string Kanji => kanji;
    [SerializeField] public string kanji;

    public string Kana => kana;
    [SerializeField] public string kana;

    public string Meaning => meaning;
    [SerializeField] public string meaning;

    public bool IsExeception;
}

public enum VerbType
{
    U, //Godan Verbs, U-Verbs
    RU, // Ichidan Verbs, RU-Verbs
    IRR, // 
}

public enum VerbConjugation
{
    Dictionary,
    Present,
    Negative,
    Past,
    PastNegative,
    Volitional,
    TeForm,
}