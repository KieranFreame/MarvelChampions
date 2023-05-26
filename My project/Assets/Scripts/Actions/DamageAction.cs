using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageAction : Action
{
    public bool Indirect { get; private set; } = false;
    public bool TargetAll { get; private set; } = false;
    public List<Health> DamageTargets { get; private set; } = new List<Health>();

    public DamageAction(AttackAction action, Health target)
    {
        Owner = action.Owner;
        Value = action.Value;
        DamageTargets.Add(target);
    }

    public DamageAction(AttackAction action, List<Health> targets)
    {
        Owner = action.Owner;
        Value = action.Value;
        DamageTargets = targets;
    }

    public DamageAction(Health target, int damage)
    {
        DamageTargets.Add(target);
        Value = damage;
    }
    
    public DamageAction(List<Health> targets, int damage, bool targetAll = false) //AOE Damage
    {
        DamageTargets = targets;
        Value = damage;
        TargetAll = targetAll;
    }
}
