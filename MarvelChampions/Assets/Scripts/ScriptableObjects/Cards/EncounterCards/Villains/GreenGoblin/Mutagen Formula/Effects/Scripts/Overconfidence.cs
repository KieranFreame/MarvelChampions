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
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;

        if (player.Identity.ActiveIdentity is Hero)
            await _owner.CharStats.InitiateAttack();
        else
            await _owner.CharStats.InitiateScheme();
    }

    private void IsTriggerMet(Action action)
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;

        if (action.Value >= 3)
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
    }
}
