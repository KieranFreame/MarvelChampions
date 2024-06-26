using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "All In A Days Work", menuName = "MarvelChampions/Card Effects/Risky Business/All In A Day's Work")]
public class DaysWork : EncounterCardEffect
{
    public override Task Resolve()
    {
        if (ScenarioManager.inst.ActiveVillain.Name == "Norman Osborn")
            RiskyBusiness.Instance.environment.AddCounters(2);
        else
            RiskyBusiness.Instance.environment.RemoveCounters(2);

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.Name == "Norman Osborn")
            RiskyBusiness.Instance.environment.AddCounters(1);
        else
            RiskyBusiness.Instance.environment.RemoveCounters(1);
        
        return Task.CompletedTask;
    }
}
