using System;
using UnityEngine;

[Serializable]
public class Monster
{
    public string Name;
    public ConjugationTypes ConjugationTypes;
    public float MaxHP;
    public Color Tint;
}

public enum MonsterDifficulty
{
    Easy,
    Medium,
    Hard,
    Boss
}