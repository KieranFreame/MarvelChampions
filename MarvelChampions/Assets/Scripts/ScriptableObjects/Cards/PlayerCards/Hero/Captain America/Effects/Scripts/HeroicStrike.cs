using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Heroic Strike", menuName = "MarvelChampions/Card Effects/Captain America/Heroic Strike")]
public class HeroicStrike : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {   
        _owner.CharStats.AttackInitiated += AttackInitiated;
        await _owner.CharStats.InitiateAttack(new(6, owner: _owner, card: Card));
    }

    private void AttackInitiated()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;

        AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
    }

    private Task AttackCompleted(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);

        if (PayCostSystem.instance.Resources.Contains(Resource.Physical) || PayCostSystem.instance.Resources.Contains(Resource.Wild))
        {
            action.Target.CharStats.Attacker.Stunned = true;
        }

        return Task.CompletedTask;
    }
}
