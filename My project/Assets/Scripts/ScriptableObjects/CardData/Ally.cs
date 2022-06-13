using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ally", menuName = "Card/Ally")]
public class Ally : PlayerCard
{
    [Header("Stats")]
    public int baseHp;
    public AttackStats combatant = new AttackStats();
    public ThwartStats thwarter;


    [Header("Status")]
    public bool stunned = false;
    public bool confused = false;
    public bool tough = false;
}
