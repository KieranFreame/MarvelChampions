using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Immortal Klaw", menuName = "MarvelChampions/Card Effects/Klaw/Immortal Klaw")]
public class ImmortalKlaw : EncounterCardEffect
{
    public override async Task Resolve()
    {
        (_card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;
        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        _owner.CharStats.Health.IncreaseMaxHealth(10);

        await Task.Yield();
    }

    public override async Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;
        _owner.CharStats.Health.IncreaseMaxHealth(-10);
        await Task.Yield();
    }
}
