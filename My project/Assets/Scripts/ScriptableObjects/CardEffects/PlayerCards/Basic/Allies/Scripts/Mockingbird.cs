using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mockingbird", menuName = "MarvelChampions/Card Effects/Basic/Mockingbird")]
public class Mockingbird : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        var action = new ApplyStatusAction(Status.Stunned, owner, new List<TargetType> { TargetType.TargetMinion, TargetType.TargetVillain });
        await ApplyStatusSystem.instance.ApplyStatus(action);
    }
}
