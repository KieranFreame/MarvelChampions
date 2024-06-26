using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Heart-Shaped Herb", menuName = "MarvelChampions/Card Effects/Nemesis/Black Panther/Heart-Shaped Herb")]
public class HeartShapedHerb : EncounterCardEffect
{
    public override Task Resolve()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

        _owner.CharStats.Health.Tough = true;
        foreach (var m in VillainTurnController.instance.MinionsInPlay)
            m.CharStats.Health.Tough = true;

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        FindObjectOfType<Villain>().CharStats.Health.Tough = true;
        await Task.Yield();
    }
}
