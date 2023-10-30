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
        {
            await owner.CharStats.InitiateScheme();
            SchemeSystem.Instance.SchemeComplete.Add(OnActivationComplete);
        }
        else //hero
        {
            await owner.CharStats.InitiateAttack();
            AttackSystem.Instance.OnAttackCompleted.Add(OnActivationComplete);
        }
    }

    private Task OnActivationComplete(Action arg0)
    {
        if (arg0 is AttackAction)
            AttackSystem.Instance.OnAttackCompleted.Remove(OnActivationComplete);
        else if (arg0 is SchemeAction)
            SchemeSystem.Instance.SchemeComplete.Remove(OnActivationComplete);

        p.Deck.Mill(arg0.Value);
        return Task.CompletedTask;
    }
}
