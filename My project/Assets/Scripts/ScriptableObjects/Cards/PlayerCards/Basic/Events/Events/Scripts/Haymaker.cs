using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Haymaker", menuName = "MarvelChampions/Card Effects/Basic/Haymaker")]
public class Haymaker : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        var action = new AttackAction(3, new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain }, owner: _owner);
        await _owner.CharStats.InitiateAttack(action);
    }
}
