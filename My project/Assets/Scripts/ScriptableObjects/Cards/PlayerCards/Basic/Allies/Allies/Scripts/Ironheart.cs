using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ironheart", menuName = "MarvelChampions/Card Effects/Basic/Ironheart")]
public class Ironheart : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        if (Card.PrevZone == Zone.Hand)
            DrawCardSystem.instance.DrawCards(new(1));

        await Task.Yield();
    }
}
