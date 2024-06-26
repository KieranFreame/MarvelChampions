using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Laser Rifle", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Experimental Weapons/Laser Rifle")]
public class LaserRifle : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        Attached = _owner = owner;
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
        await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 }, { Resource.Physical, 1 } });
        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public void Attach()
    {
        Attached.CharStats.Attacker.CurrentAttack++;
        Attached.CharStats.Attacker.Keywords.Add(Keywords.Ranged);

        Attached.Attachments.Add(this);
    }

    public void Detach()
    {
        Attached.CharStats.Attacker.CurrentAttack--;
        Attached.CharStats.Attacker.Keywords.Remove(Keywords.Ranged);

        Attached.Attachments.Remove(this);
    }
}
