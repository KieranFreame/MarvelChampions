using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Taskmaster's Sword", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Taskmaster's Sword")]
public class TaskmastersSword : AttachmentCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

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

        List<Task> tasks = new() { PayCostSystem.instance.GetResources(Resource.Scientific, 1), PayCostSystem.instance.GetResources(Resource.Physical, 1) };

        foreach (var t in tasks)
            await t;

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Attacker.Keywords.Add("Piercing");
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Attacker.Keywords.Remove("Piercing");
    }
}
