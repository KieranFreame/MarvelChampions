using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageAction : Action
{
    public bool Indirect { get; private set; } = false;
    public bool TargetAll { get; private set; } = false;
    public bool IsAttack { get; private set; } = false;
    public List<ICharacter> DamageTargets { get; private set; } = new List<ICharacter>();
    public ICard Card { get; private set; }

    public DamageAction(ICharacter target, int damage, bool isAttack = false, ICard card = null, ICharacter owner = null)
    {
        Owner = owner;
        DamageTargets.Add(target);
        Value = damage;
        Card = card;
        IsAttack = isAttack;
    }
    
    public DamageAction(List<ICharacter> targets, int damage, bool targetAll = false, bool isAttack = false, ICard card = null, ICharacter owner = null) //AOE Damage
    {
        Owner = owner;
        DamageTargets = targets;
        Value = damage;
        TargetAll = targetAll;
        Card = card;
        IsAttack = isAttack;
    }
}
