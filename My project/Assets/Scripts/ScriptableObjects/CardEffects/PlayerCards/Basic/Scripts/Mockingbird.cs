using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mockingbird", menuName = "MarvelChampions/Card Effects/Basic/Mockingbird")]
public class Mockingbird : PlayerCardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        var action = new ApplyStatusAction(Status.Stunned, owner.gameObject, new List<TargetType> { TargetType.TargetEnemy });
        card.StartCoroutine(ApplyStatusSystem.instance.ApplyStatus(action));
    }
}
