using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Kanji
{

    [field: SerializeField] public string kanji;
    [field: SerializeField] public string KunyomiReadingsFull;
    [field: SerializeField] public List<string> KunyomiReadings;
    [field: SerializeField] public string OnyomiReadingsFull;
    [field: SerializeField] public List<string> OnyomiReadings;
    [field: SerializeField] public string MeaningFull;
    [SerializeField] public List<string> Meaning;
}