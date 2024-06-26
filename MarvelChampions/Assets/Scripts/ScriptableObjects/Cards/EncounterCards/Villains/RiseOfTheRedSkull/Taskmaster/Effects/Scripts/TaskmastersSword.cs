using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Taskmaster's Sword", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Taskmaster's Sword")]
public class TaskmastersSword : AttachmentCardEffect
{
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

        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 1 }, { Resource.Physical, 1 } });

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Attacker.Keywords.Add(Keywords.Piercing);
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Attacker.Keywords.Remove(Keywords.Piercing);
    }
}
