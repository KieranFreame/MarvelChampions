using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Invasive AI", menuName = "MarvelChampions/Card Effects/Ultron/Invasive AI")]
public class InvasiveAI : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        foreach (var p in TurnManager.Players)
        {
            p.Deck.Mill(3);
        }

        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
