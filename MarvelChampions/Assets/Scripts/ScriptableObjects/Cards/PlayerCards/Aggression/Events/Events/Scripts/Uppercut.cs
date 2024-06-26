using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Uppercut", menuName = "MarvelChampions/Card Effects/Aggression/Uppercut")]
public class Uppercut : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new(5, targets: new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, AttackType.Card, owner: _owner, card:Card));
    }
}
