using System.Collections.Generic;

public interface IWord
{
    public WordType WordType { get; }
    public string Kanji { get; }
    public string Kana { get; }
    public List<string> Meaning { get; }
}
public enum WordType
{
    Noun,
    Adjective,
    Verb,
    Adverb
}