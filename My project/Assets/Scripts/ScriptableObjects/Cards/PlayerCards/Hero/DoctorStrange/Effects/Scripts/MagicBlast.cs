using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Blast", menuName = "MarvelChampions/Card Effects/Doctor Strange/Magic Blast")]
public class MagicBlast : PlayerCardEffect
{
    ICharacter target;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.IdentityName != "Doctor Strange")
                return false;
            
            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            return;
        }

        AttackSystem.TargetAcquired += TargetAcquired;
        await _owner.CharStats.InitiateAttack(new(5, owner:_owner, card:Card));

        _owner.Deck.Mill(1);
        var card = _owner.Deck.discardPile.Last() as PlayerCardData;

        Debug.Log("Discarded " + card.cardName + ". Resource is " + card.cardResources[0].ToString());

        switch (card.cardResources[0])
        {
            case Resource.Physical:
                PhysicalEffect();
                break;
            case Resource.Energy:
                EnergyEffect();
                break;
            case Resource.Scientific:
                ScientificEffect();
                break;
            case Resource.Wild:
                PhysicalEffect();
                EnergyEffect();
                ScientificEffect();
                break;
            default:
                Debug.LogError("Top card doesn't have a resource");
                break;
        }
    }

    private void ScientificEffect()
    {
        target.CharStats.Schemer.Confused = true;
    }

    private void EnergyEffect()
    {
        target.CharStats.Health.TakeDamage(new(target, 2, true, card: Card, owner: _owner));
    }

    private void PhysicalEffect()
    {
        target.CharStats.Attacker.Stunned = true;
    }

    private void TargetAcquired(ICharacter arg0) => target = arg0;
    
}
