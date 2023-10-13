using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Collapsing Bridge", menuName = "MarvelChampions/Card Effects/Risky Business/Collapsing Bridge")]
public class CollapsingBridge : EncounterCardEffect
{
    Crisis _crisis;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _crisis = new(card as SchemeCard);
        return Task.CompletedTask;
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
