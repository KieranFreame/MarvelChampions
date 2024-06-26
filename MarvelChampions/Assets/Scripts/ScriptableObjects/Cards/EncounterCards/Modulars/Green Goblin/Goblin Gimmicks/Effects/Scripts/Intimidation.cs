using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Intimidation", menuName = "MarvelChampions/Card Effects/Modulars/Goblin Gimmicks/Intimidation")]
public class Intimidation : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.ResourcesAvailable() >= 2)
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Spend 2 resources", "Give the villain a face-down boost card" });

            if (decision == 1)
            {
                await PayCostSystem.instance.GetResources(new() { { Resource.Any, 2 } });
                return;
            }
        }

        BoostSystem.Instance.BoostCardCount++;
    }

    public override Task Boost(Action action)
    {
        BoostSystem.Instance.BoostCardCount++;
        return Task.CompletedTask;
    }
}
