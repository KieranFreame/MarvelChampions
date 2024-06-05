using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Regenerative Healing", menuName = "MarvelChampions/Card Effects/Modulars/Goblin Gimmicks/Regenerative Healing")]
public class RegenerativeHealing : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (owner.CharStats.Health.Damaged())
        {
            owner.CharStats.Health.CurrentHealth += owner.Stages.Stage * 2;

            return Task.CompletedTask;
        }

        ScenarioManager.inst.Surge(player);
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        ScenarioManager.inst.ActiveVillain.CharStats.Health.CurrentHealth += 2;
        return Task.CompletedTask;
    }
}
