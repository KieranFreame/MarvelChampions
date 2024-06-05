using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "For Justice", menuName = "MarvelChampions/Card Effects/Justice/For Justice")]
public class ForJustice : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        return ScenarioManager.inst.ThreatPresent() && _owner.Identity.ActiveIdentity is Hero && base.CanBePlayed();
    }

    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateThwart(new(PayCostSystem.instance.Resources.Contains(Resource.Scientific) ? 4 : 3, Owner));
    }
}
