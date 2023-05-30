using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageAction : Action
{
    public bool Indirect { get; private set; } = false;
    public bool TargetAll { get; private set; } = false;
    public List<ICharacter> DamageTargets { get; private set; } = new List<ICharacter>();

    public DamageAction(AttackAction action, ICharacter target)
    {
        Owner = action.Owner;
        Value = action.Value;
        DamageTargets.Add(target);
    }

    public DamageAction(AttackAction action, List<ICharacter> targets)
    {
        Owner = action.Owner;
        Value = action.Value;
        DamageTargets = targets;
    }

    public DamageAction(ICharacter target, int damage)
    {
        DamageTargets.Add(target);
        Value = damage;
    }
    
    public DamageAction(List<ICharacter> targets, int damage, bool targetAll = false) //AOE Damage
    {
        DamageTargets = targets;
        Value = damage;
        TargetAll = targetAll;
    }
}
