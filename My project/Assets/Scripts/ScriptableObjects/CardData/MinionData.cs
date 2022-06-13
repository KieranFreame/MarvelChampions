using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Card/Minion")]
public class MinionData : CardData
{
    [Header("Stats")]
    public AttackStats combatant = new AttackStats();
    public int baseScheme;
    public int baseHealth;

    [Header("Status")]
    public bool stunned = false;
    public bool confused = false;
    public bool tough = false;
}
