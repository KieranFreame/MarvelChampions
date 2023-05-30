using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAction : Action
{
    List<IExhaust> targets;

    public ReadyAction(List<IExhaust> _targets, GameObject owner)
    {
        Owner = owner;
        targets = _targets;
    }

    public void Execute()
    {
        Owner.StartCoroutine(TargetSystem.instance.SelectTarget(targets, target => { target.Ready(); }));
    }
}
