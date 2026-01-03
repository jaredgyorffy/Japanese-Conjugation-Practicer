using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/KanaRomajiList", fileName = "KanaRomajiList")]
public class KanaRomajiList : ScriptableObject
{
    public List<KanaRomajiPair> SingleLetterPairs;
    public List<KanaRomajiPair> TwoLetterPairs;
    public List<KanaRomajiPair> ThreeLetterPairs;
}
