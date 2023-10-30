using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "For Justice", menuName = "MarvelChampions/Card Effects/Justice/For Justice")]
public class ForJustice : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.ActiveIdentity is AlterEgo)
                return false;

            return ScenarioManager.sideSchemes.Count > 0 || ScenarioManager.inst.MainScheme.Threat.CurrentThreat > 0;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (PayCostSystem.instance.Resources.Contains(Resource.Scientific))
        {
            await _owner.CharStats.InitiateThwart(new(4));
        }
        else
        {
            await _owner.CharStats.InitiateThwart(new(3));
        }
    }
}
