using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WordList", menuName = "Scriptable Objects/VerbList")]
public class VerbList : ScriptableObject
{
    [field: SerializeField] public string listName { get; private set; }
    [field: SerializeField][FormerlySerializedAs("List")] public List<Verb> verbList;
    [field: SerializeField] public List<Adjective> adjectiveList;
    [field: SerializeField] public List<Noun> nounList;
}