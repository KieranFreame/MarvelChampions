using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAction : Action
{
    public ReadyAction(List<TargetType> targets, GameObject owner)
    {
        Owner = owner;
        Targets.AddRange(targets);
    }

    public IEnumerator Execute()
    {
        yield return Owner.StartCoroutine(TargetSystem.instance.GetTarget<IExhaust>(this, exhaust => { exhaust.Ready(); }));
    }
}
