using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Imminent Overload", menuName = "MarvelChampions/Card Effects/Nemesis/Iron Man/Imminent Overload")]
public class ImminentOverload : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        (card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);
        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        await Task.Yield();
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;
        return Task.CompletedTask;
    }
}
