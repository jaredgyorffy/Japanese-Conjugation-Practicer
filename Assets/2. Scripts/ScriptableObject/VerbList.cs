using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordList", menuName = "Scriptable Objects/VerbList")]
public class VerbList : ScriptableObject
{
    [field: SerializeField] public string listName { get; private set; }
    public List<Verb> List;
}
