using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : Action
{
    public int heal;
    public bool targetPlayer;

    public HealAction(ActionData data) : base (data){}

    public override void Execute() => HealSystem.instance.InitiateHeal(this);
}
