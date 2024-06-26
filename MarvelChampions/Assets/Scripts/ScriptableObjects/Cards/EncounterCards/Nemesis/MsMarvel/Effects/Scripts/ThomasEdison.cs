using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Thomas Edison", menuName = "MarvelChampions/Card Effects/Nemesis/Ms Marvel/Thomas Edison")]
public class ThomasEdison : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        (_card as MinionCard).CharStats.Health.Modifiers.Add(ModifyDamage);
        return Task.CompletedTask;
    }

    private Task<DamageAction> ModifyDamage(DamageAction action)
    {
        if (VillainTurnController.instance.MinionsInPlay.Count > 1) //not just Thomas Edison
        {
            action.Value = 0;
        }

        return Task.FromResult(action);
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(ModifyDamage);
        return Task.CompletedTask;
    }
}
