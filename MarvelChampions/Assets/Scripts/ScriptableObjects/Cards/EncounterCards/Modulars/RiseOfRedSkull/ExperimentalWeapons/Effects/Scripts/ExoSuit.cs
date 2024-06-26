using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Exo-Suit", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Experimental Weapons/Exo-Suit")]
public class ExoSuit : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }

    readonly Dictionary<Resource, int> cost = new()
    {
        {Resource.Energy, 1},
        {Resource.Physical, 1},
        {Resource.Scientific, 1}
    };

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Energy) && player.HaveResource(Resource.Physical) && player.HaveResource(Resource.Scientific));
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(cost);
        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public void Attach()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Schemer.CurrentScheme++;

        _owner.Attachments.Add(this);
    }

    public void Detach()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Schemer.CurrentScheme--;

        _owner.Attachments.Remove(this);
    }
}
