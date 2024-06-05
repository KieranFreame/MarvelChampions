using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sneak By", menuName = "MarvelChampions/Card Effects/Ms Marvel/Sneak By")]
public class SneakBy : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return ScenarioManager.sideSchemes.Count > 0 || ScenarioManager.inst.MainScheme.Threat.CurrentThreat > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateThwart(new(3, Owner));
    }
}
