using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Images of Ikonn", menuName = "MarvelChampions/Card Effects/Doctor Strange/Invocations/Images of Ikonn")]
public class ImagesIkonn : PlayerCardEffect, IInvocation
{
    public override bool CanActivate()
    {
        if (ScenarioManager.inst.ActiveVillain.CharStats.Schemer.Confused && !ScenarioManager.inst.ThreatPresent())
            return false;

        return true;
    }

    public async Task Special()
    {
        ICharacter villain = ScenarioManager.inst.ActiveVillain;

        villain.CharStats.Schemer.Confused = true;

        List<SchemeCard> schemes = new() { ScenarioManager.inst.MainScheme };
        schemes.AddRange(ScenarioManager.sideSchemes);

        SchemeCard target = (schemes.Count > 1) ? await TargetSystem.instance.SelectTarget(schemes) : schemes[0];
        target.Threat.RemoveThreat(4);
    }
}
