using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair Sequence (Boost)", menuName = "MarvelChampions/Card Effects/Ultron/Repair Sequence (Boost)")]
public class RepairSequenceBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        int heal = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();

        ScenarioManager.inst.ActiveVillain.CharStats.Health.CurrentHealth += heal;

        return Task.CompletedTask;
    }
}
