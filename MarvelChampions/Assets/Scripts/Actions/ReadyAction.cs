using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAction : Action
{
    List<IExhaust> targets;

    public ReadyAction(List<IExhaust> _targets, ICharacter owner)
    {
        Owner = owner;
        targets = _targets;
    }

    public async void Execute()
    {
        IExhaust target = await TargetSystem.instance.SelectTarget(targets);
        target.Ready();
    }
}
