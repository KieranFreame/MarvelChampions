using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Edison's Giant Robot", menuName = "MarvelChampions/Card Effects/Nemesis/Ms Marvel/Edison's Giant Robot")]
public class EdisonsGiantRobot : EncounterCardEffect
{
    bool CanBeDamaged;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        CanBeDamaged = false;

        (Card as MinionCard).CharStats.Health.Modifiers.Add(ModifyDamage);

        return Task.CompletedTask;
    }

    private Task<DamageAction> ModifyDamage(DamageAction action)
    {
        if (!CanBeDamaged)
            action.Value = 0;

        return Task.FromResult(action);
    }

    public override bool CanActivate(Player player)
    {
        if (CanBeDamaged) //already activated
            return false;

        if (!player.HaveResource(Resource.Scientific))
            return false;

        return true;
    }

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(Resource.Scientific, 1);

        CanBeDamaged = true;

        TurnManager.OnEndPlayerPhase += EndOfPhase;
    }

    private void EndOfPhase()
    {
        CanBeDamaged = false;

        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(ModifyDamage);
        TurnManager.OnEndPlayerPhase -= EndOfPhase;

        return Task.CompletedTask;
    }
}
