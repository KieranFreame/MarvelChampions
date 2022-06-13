using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter Set", fileName = "New Encounter Set")]
public class EncounterSet : ScriptableObject
{
    public List<CardData> cards;
}
