using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Avengers Mansion", menuName = "MarvelChampions/Card Effects/Basic/Avengers Mansion")]
public class AvengersMansion : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        DrawCardSystem.instance.DrawCards(new(1, _owner));

        await Task.Yield();
    }
}
