using System;
using UnityEngine;

[Serializable]
public class Monster
{
    public string Name;
    public ConjugationTypes ConjugationTypes;
    public float MaxHP;
    public Color Tint;
    public MonsterType MonsterType;
}

public enum MonsterType
{
    Slime = 0,
    Skeleton = 1,
}

public enum MonsterDifficulty
{
    Easy,
    Medium,
    Hard,
    Boss
}