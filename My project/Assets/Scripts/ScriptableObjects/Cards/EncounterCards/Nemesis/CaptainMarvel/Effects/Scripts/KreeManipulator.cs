using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Kree Manipulator", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Kree Manipulator")]
public class KreeManipulator : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        ScenarioManager.inst.Surge(player);

        ScenarioManager.inst.MainScheme.Threat.GainThreat(1);

        await Task.Yield();
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction) return;
        if (DefendSystem.Instance.Target != null) return;

        ScenarioManager.inst.MainScheme.Threat.GainThreat(1);

        await Task.Yield();
    }
}
