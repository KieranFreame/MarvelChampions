using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Power Gauntlets", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Experimental Weapons/Power Gauntlets")]
public class PowerGauntlets : EncounterCardEffect, IAttachment
{
    public ICharacter Attached { get; set; }
    Player _target;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        Attach();

        return Task.CompletedTask;
    }

    public override bool CanActivate(Player player)
    {
        return (player.HaveResource(Resource.Physical) && player.HaveResource(Resource.Scientific));
    }

    public override async Task Activate(Player player)
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Physical, 1 }, { Resource.Scientific, 1 } });
        Detach();
        ScenarioManager.inst.EncounterDeck.Discard(Card);
    }

    public void Attach()
    {
        _owner.CharStats.AttackInitiated += InitiateAttack;

        _owner.Attachments.Add(this);
    }

    
    public void Detach()
    {
        _owner.CharStats.AttackInitiated -= InitiateAttack;

        _owner.Attachments.Remove(this);
    }

    private void InitiateAttack()
    {
        DefendSystem.Instance.OnTargetSelected += SetDefender;
    }

    private void SetDefender(ICharacter arg0)
    {
        DefendSystem.Instance.OnTargetSelected -= SetDefender;

        if (arg0 is Player)
        {
            _target = arg0 as Player;
            _target.CharStats.Health.OnTakeDamage += AttackComplete;
        }
    }

    void AttackComplete(DamageAction action)
    {
        _target.CharStats.Health.OnTakeDamage -= AttackComplete;

        if (action.Value > 0)
        {
            var pCard = _target.Hand.cards[Random.Range(0, _target.Hand.cards.Count)];

            Debug.Log("Discarding" + pCard.CardName);

            _target.Hand.Discard(pCard);
        }
    }
}
