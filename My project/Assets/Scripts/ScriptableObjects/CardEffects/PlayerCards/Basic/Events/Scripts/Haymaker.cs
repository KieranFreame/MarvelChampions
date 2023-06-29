using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Haymaker", menuName = "MarvelChampions/Card Effects/Basic/Haymaker")]
public class Haymaker : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        var action = new AttackAction(3, new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain }, owner: _owner);
        await player.CharStats.InitiateAttack(action);
    }
}
