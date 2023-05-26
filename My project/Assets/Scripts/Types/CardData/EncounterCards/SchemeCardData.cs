using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scheme", menuName = "MarvelChampions/Card/Scheme")]
public class SchemeCardData : EncounterCardData
{
    public bool IsMainScheme;

    [Header("Scheme")]
    public int StartingThreat;

    [Header("Main Scheme")]
    public int MaximumThreat;
    public int Acceleration;
}
