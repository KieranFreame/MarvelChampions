using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackStats
{
    public int baseAttack;
    public int attack;
    public int atkConsq;

    public AttackStats()
    {
        attack = baseAttack;
    }
}
