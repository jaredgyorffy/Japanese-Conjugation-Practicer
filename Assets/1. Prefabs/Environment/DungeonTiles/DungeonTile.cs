using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : MonoBehaviour
{
    [field: SerializeField] public Transform Start { get; private set; }
    [field: SerializeField] public Transform Center { get; private set; }
    [field: SerializeField] public List<DungeonEndPoint> Endpoints {get; private set;}
}

[Serializable]
public class DungeonEndPoint
{
    public DungeonDirection Direction;
    public Transform Endpoint;
}