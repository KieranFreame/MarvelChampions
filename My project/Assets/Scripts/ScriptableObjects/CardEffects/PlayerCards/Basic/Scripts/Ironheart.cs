using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ironheart", menuName = "MarvelChampions/Card Effects/Basic/Ironheart")]
public class Ironheart : CardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        if (card.PrevZone == Zone.Hand)
            DrawCardSystem.instance.DrawCards(new(1));
    }
}
