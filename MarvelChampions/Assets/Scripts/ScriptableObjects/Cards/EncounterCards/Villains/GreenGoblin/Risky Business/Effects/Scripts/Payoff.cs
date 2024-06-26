using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Payoff", menuName = "MarvelChampions/Card Effects/Risky Business/Payoff")]
public class Payoff : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.Name == "Norman Osborn")
        {
            RiskyBusiness.Instance.environment.AddCounters(1);
        }
        else
        {
            RiskyBusiness.Instance.environment.RemoveCounters(1);
        }

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;
        return Task.CompletedTask;
    }
}
