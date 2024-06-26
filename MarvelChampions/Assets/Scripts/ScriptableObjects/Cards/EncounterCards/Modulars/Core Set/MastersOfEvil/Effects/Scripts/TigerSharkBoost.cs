using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Main Effect: After Tiger Shark attacks, he gains Tough.
/// Boost Effect: The villain gains Tough
/// </summary>

[CreateAssetMenu(fileName = "Tiger Shark (Boost)", menuName = "MarvelChampions/Card Effects/Masters of Evil/Tiger Shark (Boost)")]
public class TigerSharkBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
