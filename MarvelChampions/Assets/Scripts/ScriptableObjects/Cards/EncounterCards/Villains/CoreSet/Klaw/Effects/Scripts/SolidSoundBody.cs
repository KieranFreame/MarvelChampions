using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Solid-Sound Body", menuName = "MarvelChampions/Card Effects/Klaw/Solid-Sound Body")]
public class SolidSoundBody : EncounterCardEffect
{
    Retaliate retaliate;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        retaliate = new(owner, 1);
        
        await Task.Yield();
    }

    public override bool CanActivate(Player p)
    {
        return (p.HaveResource(Resource.Energy) && p.HaveResource(Resource.Scientific) && p.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player p)
    {
        List<Task> tasks = new()
        {
            PayCostSystem.instance.GetResources(Resource.Energy, 1),
            PayCostSystem.instance.GetResources(Resource.Scientific, 1),
            PayCostSystem.instance.GetResources(Resource.Physical, 1)
        };

        await Task.WhenAll(tasks);

        retaliate.WhenRemoved();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }
}
