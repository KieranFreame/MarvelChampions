using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageAction : Action
{
    public DamageAction(ActionData data) : base(data){}

    public override void Execute() => DamageSystem.instance.InitiateDamage(this);
}
