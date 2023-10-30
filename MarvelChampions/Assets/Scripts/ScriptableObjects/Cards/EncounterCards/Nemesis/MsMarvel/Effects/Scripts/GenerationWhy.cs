using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Generation Why?", menuName = "MarvelChampions/Card Effects/Nemesis/Ms Marvel/Generation Why?")]
public class GenerationWhy : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        foreach (var p in TurnManager.Players)
        {
            int count = p.CardsInPlay.Allies.Count + p.CardsInPlay.Permanents.Where(x => x.CardTraits.Contains("Persona")).Count();
            p.Deck.Mill(count);
        }

        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;
        VillainTurnController.instance.HazardCount++;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
