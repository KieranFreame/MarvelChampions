using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Haymaker", menuName = "MarvelChampions/Card Effects/Basic/Haymaker")]
public class Haymaker : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new AttackAction(3, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, attackType: AttackType.Card, owner: _owner, card: Card));
    }
}
