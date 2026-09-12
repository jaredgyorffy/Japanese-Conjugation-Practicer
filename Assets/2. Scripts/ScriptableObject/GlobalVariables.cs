using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/GlobalVariables", fileName = "GlobalVariables", order = 0)]
public class GlobalVariables : ScriptableObject
{
    [field: SerializeField] public List<DataSource> WordLists { get; private set; }
}
