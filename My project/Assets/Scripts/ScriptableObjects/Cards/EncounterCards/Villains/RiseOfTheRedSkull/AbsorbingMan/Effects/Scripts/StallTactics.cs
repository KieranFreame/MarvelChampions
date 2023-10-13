using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stall Tactics", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Stall Tactics")]
public class StallTactics : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        int threat = (NoneShallPass.delay.CountersLeft/2);

        if (threat > 0)
        {
            ScenarioManager.inst.Surge(player);
        }
        else
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(threat);
        }

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        if (NoneShallPass.delay.CountersLeft >= 5)
        {
            List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
            targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, 1);
        }
    }
}
