using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction
{
    public ICombatant attacker;
    public IDestructable target;
    public List<string> keywords;

    public AttackAction(ICombatant attacker, IDestructable target, List<string> keywords)
    {
        this.attacker = attacker;
        this.target = target;
        this.keywords = keywords;
    }
}
