using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordList", menuName = "Scriptable Objects/VerbList")]
public class VerbList : ScriptableObject
{
    public List<Verb> List;
}
