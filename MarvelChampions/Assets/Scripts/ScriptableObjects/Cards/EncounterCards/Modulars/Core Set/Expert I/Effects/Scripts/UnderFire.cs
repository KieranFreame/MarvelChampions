using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Under Fire", menuName = "MarvelChampions/Card Effects/Expert/Under Fire")]
public class UnderFire : EncounterCardEffect
{
    public override Task Resolve()
    {
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
        return Task.CompletedTask;
    }
}
