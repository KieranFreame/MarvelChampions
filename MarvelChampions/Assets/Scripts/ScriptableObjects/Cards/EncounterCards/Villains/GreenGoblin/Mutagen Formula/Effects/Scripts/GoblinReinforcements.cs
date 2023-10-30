using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin Reinforcements", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Goblin Reinforcements")]
public class GoblinReinforcements : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        (card as SchemeCard).Threat.GainThreat(VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Goblin")).Count());

        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;
        return Task.CompletedTask;
    }
}
