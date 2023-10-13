using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Technological Enhancements", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Technological Enhancements")]
public class TechnologicalEnhancements : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.MainScheme.Threat.GainThreat(1); //Incite 1

        ScenarioManager.inst.MainScheme.GetComponent<Counters>().AddCounters(1);

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        ScenarioManager.inst.MainScheme.GetComponent<Counters>().AddCounters(1);
        return Task.CompletedTask;
    }
}
