using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Laser Rifle", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Experimental Weapons/Laser Rifle")]
public class LaserRifle : AttachmentCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();
        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Energy) && player.HaveResource(Resource.Physical));
    }

    public override async Task Activate(Player player)
    {
        List<Task> tasks = new List<Task>() 
        { 
            PayCostSystem.instance.GetResources(Resource.Energy, 1),
            PayCostSystem.instance.GetResources(Resource.Physical, 1)
        };

        foreach (Task t in tasks)
        {
            await t;
        }

        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public override void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Attacker.Keywords.Add(Keywords.Ranged);

        _owner.Attachments.Add(Card as AttachmentCard);
    }

    public override void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Attacker.Keywords.Remove(Keywords.Ranged);

        _owner.Attachments.Remove(Card as AttachmentCard);
    }
}
