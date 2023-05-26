using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter Card", menuName = "MarvelChampions/Card/Encounter Card")]
public class EncounterCardData : CardData
{
    [Header("Encounter Card")]
    public int boostIcons;
    public string boostDescription;
}
