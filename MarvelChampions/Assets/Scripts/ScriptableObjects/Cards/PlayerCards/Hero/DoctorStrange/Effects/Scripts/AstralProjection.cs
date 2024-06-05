using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Astral Projection", menuName = "MarvelChampions/Card Effects/Doctor Strange/Astral Projection")]
public class AstralProjection : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.IdentityName != "Doctor Strange")
                return false;

            if (!ScenarioManager.inst.ThreatPresent())
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Thwarter.Confused)
        {
            _owner.CharStats.Thwarter.Confused = false;
            return;
        }

        EncounterCardData data = ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData;
        Debug.Log($"Revealing {data.cardName}. Adding {data.boostIcons} to this thwart.");

        await ThwartSystem.Instance.InitiateThwart(new(3 + data.boostIcons, Owner));

        return;
    }
}
