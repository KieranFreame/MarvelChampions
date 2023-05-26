using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "MarvelChampions/Card/Minion")]
public class MinionCardData : EncounterCardData
{
    [Header("Stats")]
    public int baseAttack;
    public int baseScheme;
    public int baseHealth;
}
