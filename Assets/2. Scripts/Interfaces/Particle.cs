using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Particle
{
    public string Kana => kana;
    [SerializeField] private string kana;
    public string MeaningFull => meaningFull;
    [SerializeField] private string meaningFull;
    public List<string> Meaning => meaning;
    [SerializeField] private List<string> meaning;
}