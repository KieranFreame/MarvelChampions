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

        _owner.Surge(player);

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().GainThreat(1);

        await Task.Yield();
    }

    public override async Task Boost(Action action)
    {
        if (action is not AttackAction) return;
        if (DefendSystem.instance.Target != null) return;

        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().GainThreat(1);

        await Task.Yield();
    }
}
