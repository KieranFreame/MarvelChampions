using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb Scare", menuName = "MarvelChampions/Card Effects/Bomb Scare/Bomb Scare")]
public class BombScare : EncounterCardEffect
{
    /// <summary>
    /// When Revealed: Place an additional 1 threat here per player
    /// Acceleration Icon
    /// </summary>

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        (card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);
        ScenarioManager.inst.MainScheme.Threat.Acceleration += 1;
        await Task.Yield();
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration -= 1;

        return Task.CompletedTask;
    }
}
