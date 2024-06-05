using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Rage of Ultron", menuName = "MarvelChampions/Card Effects/Ultron/Rage of Ultron")]
public class RageOfUltron : EncounterCardEffect
{
    Player p;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        p = player;

        if (player.Identity.ActiveIdentity is AlterEgo)
            await owner.CharStats.InitiateScheme();
        else
            await owner.CharStats.InitiateAttack();

        EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override Task Resolve()
    {
        int mill = TurnManager.instance.CurrPlayer.Identity.ActiveIdentity is AlterEgo ? SchemeSystem.Instance.Action.Value : AttackSystem.Instance.Action.Value;

        p.Deck.Mill(mill);
        return Task.CompletedTask;
    }
}
