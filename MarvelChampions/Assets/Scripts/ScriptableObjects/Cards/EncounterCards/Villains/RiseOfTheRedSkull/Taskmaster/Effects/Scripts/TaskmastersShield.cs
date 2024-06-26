using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Taskmaster's Shield", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Taskmaster's Shield")]
public class TaskmastersShield: AttachmentCardEffect
{
    Retaliate _retaliate;

    public override Task OnEnterPlay()
    {
        Attach();
        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        if (player.Exhausted)
            return false;

        if (!player.HaveResource(Resource.Scientific) || !player.HaveResource(Resource.Physical))
            return false;

        return true;
    }

    public override async Task Activate(Player player)
    {
        player.Exhaust();

        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 1 },{ Resource.Physical, 1 } });

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        _retaliate = new(_owner, 1);
    }

    public override void Detach()
    {
        _retaliate.WhenRemoved();
    }
}
