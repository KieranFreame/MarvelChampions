using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Psyche-Magnitron", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/The Psyche-Magnitron")]
public class PsycheMagnitron : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);

        VillainTurnController.instance.HazardCount++;

        await Task.Yield();
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
