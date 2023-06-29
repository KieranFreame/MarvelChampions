using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiderWoman", menuName = "MarvelChampions/Card Effects/Captain Marvel/Spider-Woman")]
public class SpiderWoman : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        var action = new ApplyStatusAction(Status.Confused, player, new List<TargetType>() { TargetType.TargetVillain });
        await ApplyStatusSystem.instance.ApplyStatus(action);
    }
}
