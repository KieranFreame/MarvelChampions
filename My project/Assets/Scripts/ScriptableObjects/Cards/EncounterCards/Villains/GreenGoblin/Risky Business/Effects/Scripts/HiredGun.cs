using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hired Gun", menuName = "MarvelChampions/Card Effects/Risky Business/Hired Gun")]
public class HiredGun : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (owner.VillainName != "Norman Osborn")
        {
            BoostSystem.Instance.BoostCardCount++;
            return;
        }
        else
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Give the villain a face-down boost card", "Place 2 counters on the environment" });

            if (decision == 1)
            {
                BoostSystem.Instance.BoostCardCount++;
                return;
            }
        }

        RiskyBusiness.Instance.environment.AddCounters(2);
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.VillainName == "Norman Osborn")
        {
            RiskyBusiness.Instance.environment.AddCounters(1);
        }
        else
        {
            RiskyBusiness.Instance.environment.RemoveCounters(1);
        }

        return Task.CompletedTask;
    }
}
