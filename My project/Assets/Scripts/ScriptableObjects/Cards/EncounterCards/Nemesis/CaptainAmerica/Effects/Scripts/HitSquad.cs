using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hit Squad", menuName = "MarvelChampions/Card Effects/Nemesis/Captain America/Hit Squad")]
public class HitSquad : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        foreach (var p in TurnManager.Players)
        {
            int damage = (ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData).boostIcons;
            ScenarioManager.inst.EncounterDeck.Mill(1);

            p.CharStats.Health.TakeDamage(new(p, damage, card:card, owner:owner));
        }

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;
        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;
        
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        return Task.CompletedTask;
    }
}
