using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Solid-Sound Body", menuName = "MarvelChampions/Card Effects/Klaw/Solid-Sound Body")]
public class SolidSoundBody : EncounterCardEffect
{
    Retaliate retaliate;

    public override Task Resolve()
    {
        retaliate = new(_owner, 1);
        
        return Task.CompletedTask;
    }

    public override bool CanActivate(Player p)
    {
        return (p.HaveResource(Resource.Energy) && p.HaveResource(Resource.Scientific) && p.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player p)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 }, { Resource.Scientific, 1 }, { Resource.Physical, 1 } });
        retaliate.WhenRemoved();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
