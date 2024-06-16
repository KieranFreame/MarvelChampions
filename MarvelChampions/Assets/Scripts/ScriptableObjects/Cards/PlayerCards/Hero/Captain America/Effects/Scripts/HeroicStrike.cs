using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

[CreateAssetMenu(fileName = "Heroic Strike", menuName = "MarvelChampions/Card Effects/Captain America/Heroic Strike")]
public class HeroicStrike : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {   
        if (await _owner.CharStats.InitiateAttack(new(6, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, attackType: AttackType.Card, owner: _owner, card: Card)))
        {
            ICharacter target = AttackSystem.Instance.Action.Target;

            if (target != null && (PayCostSystem.instance.Resources.Contains(Resource.Physical) || PayCostSystem.instance.Resources.Contains(Resource.Wild)))
            {
                target.CharStats.Attacker.Stunned = true;
            }
        }
    }
}
