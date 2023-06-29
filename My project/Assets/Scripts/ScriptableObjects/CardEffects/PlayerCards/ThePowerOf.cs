using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "The Power of Aspect", menuName = "MarvelChampions/Card Effects/The Power Of")]
public class ThePowerOf : ResourceCardEffect
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
    }

    public override List<Resource> GetResources()
    {
        if (PlayCardSystem.instance.CardToPlay.CardAspect == Card.CardAspect)
        {
            return new List<Resource>() { Card.Resources[0], Card.Resources[0] };
        }

        return Card.Resources;
    }
}
