using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Momentum Shift", menuName = "MarvelChampions/Card Effects/Protection/Events/Momentum Shift")]
public class MomentumShift : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.ActiveIdentity is not Hero)
                return false;

            if (!_owner.CharStats.Health.Damaged())
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        _owner.CharStats.AttackInitiated += AttackInitiated;
        await _owner.CharStats.InitiateAttack(new(2, new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, AttackType.Card, owner: _owner));
        _owner.CharStats.AttackInitiated -= AttackInitiated;
    }

    private void AttackInitiated()
    {
        _owner.CharStats.Health.CurrentHealth += 2;
    }
}
