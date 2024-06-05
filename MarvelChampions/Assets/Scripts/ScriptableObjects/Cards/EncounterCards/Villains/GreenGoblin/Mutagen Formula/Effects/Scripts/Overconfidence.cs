using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Alter-Ego: Goblin schemes. If at least 3 threat was placed this way, Surge.
/// Hero: Goblin attacks. If at least 3 damage was dealt this way, Surge.
/// </summary>

[CreateAssetMenu(fileName = "Overconfidence", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Overconfidence")]
public class Overconfidence : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is Hero)
        {
            AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
            await owner.CharStats.InitiateAttack();
        }
        else
        {
            SchemeSystem.Instance.SchemeComplete.Add(IsTriggerMet);
            await owner.CharStats.InitiateScheme();
        }
    }

    private void IsTriggerMet(Action action)
    {
        if (action.Value >= 3)
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
    }
}
