using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : MonoBehaviour
{
    [field: SerializeField] public Transform Start { get; private set; }
    [field: SerializeField] public Transform Center { get; private set; }
    [SerializeField] private Transform endPointParent;
    [field: SerializeField] public List<Transform> Endpoints {get; private set;}

    private void Awake()
    {
        Endpoints = new();
        for (int i = 0; i < endPointParent.childCount; i++)
        {
            Endpoints.Add(endPointParent.GetChild(i));
        }
    }
}
