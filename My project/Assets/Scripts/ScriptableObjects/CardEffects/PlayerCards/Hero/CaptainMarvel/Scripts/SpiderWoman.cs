using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiderWoman", menuName = "MarvelChampions/Card Effects/Captain Marvel/Spider-Woman")]
public class SpiderWoman : CardEffect
{
    public override void OnEnterPlay(Player player, Card card)
    {
        var action = new ApplyStatusAction(Status.Confused, card.gameObject, new List<TargetType>() { TargetType.TargetVillain });
        card.StartCoroutine(ApplyStatusSystem.instance.ApplyStatus(action));
    }
}
