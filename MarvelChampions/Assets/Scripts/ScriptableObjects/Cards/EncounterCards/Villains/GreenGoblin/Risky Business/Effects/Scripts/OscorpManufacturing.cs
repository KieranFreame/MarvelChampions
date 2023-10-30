using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Oscorp Manufacturing", menuName = "MarvelChampions/Card Effects/Risky Business/Oscorp Manufacturing")]
public class OscorpManufacturing : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (owner.VillainName == "Norman Osborn")
        {
            (card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);
        }

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        return Task.CompletedTask;
    }
}
