using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Big Hands", menuName = "MarvelChampions/Card Effects/Ms Marvel/Big Hands")]
public class BigHands : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new(4, new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, AttackType.Card, owner:_owner));
    }
}
