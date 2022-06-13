using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    public Attacker owner;
    public int damage;

    public AttackAction(int attack = 0, Attacker owner = null) : base ("AttackAction")
    {
        this.owner = owner;
        if (this.owner != null)
            this.damage = owner._attack;
        else
            value = attack;
    }

    public AttackAction(ActionData data) : base(data) { }

    public override void Execute() => AttackSystem.instance.InitiateAttack(this);
}
