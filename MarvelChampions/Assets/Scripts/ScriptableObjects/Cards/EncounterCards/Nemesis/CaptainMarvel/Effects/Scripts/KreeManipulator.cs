using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Kree Manipulator", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Kree Manipulator")]
public class KreeManipulator : EncounterCardEffect
{
    public override Task Resolve()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

        ScenarioManager.inst.MainScheme.Threat.GainThreat(1);

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction) return;
        if (DefendSystem.Instance.Target != null) return;

        ScenarioManager.inst.MainScheme.Threat.GainThreat(1);

        await Task.Yield();
    }
}
