using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Crossbones' Assault", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Crossbones' Assault")]
public class CrossbonesAssault : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration++;
        return Task.CompletedTask;
    }

    public override async Task WhenDefeated()
    {
        if (TurnManager.instance.CurrPlayer.Identity.ActiveIdentity is AlterEgo)
            await _owner.CharStats.InitiateScheme();
        else
            await _owner.CharStats.InitiateAttack();

        ScenarioManager.inst.MainScheme.Threat.Acceleration--;
    }
}
