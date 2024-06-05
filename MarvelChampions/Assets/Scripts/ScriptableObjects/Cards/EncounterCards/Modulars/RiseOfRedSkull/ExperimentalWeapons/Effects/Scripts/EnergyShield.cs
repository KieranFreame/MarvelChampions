using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Shield", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Experimental Weapons/Energy Shield")]
public class EnergyShield : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    Retaliate _retaliate;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Energy) && player.HaveResource(Resource.Scientific));
    }

    public override async Task Activate(Player player)
    {
        List<Task> tasks = new List<Task>()
        {
            PayCostSystem.instance.GetResources(Resource.Energy, 1),
            PayCostSystem.instance.GetResources(Resource.Scientific, 1)
        };

        foreach (Task t in tasks)
        {
            await t;
        }

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public void Attach()
    {
        _retaliate = new(_owner, 1);
        _owner.Attachments.Add(this);
    }

    public void Detach()
    {
        _retaliate.WhenRemoved();
        _owner.Attachments.Remove(this);
    }
}
