using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Haymaker", menuName = "MarvelChampions/Card Effects/Basic/Haymaker")]
public class Haymaker : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        var action = new AttackAction(3, owner: _owner, card:Card);
        await _owner.CharStats.InitiateAttack(action);
    }
}
