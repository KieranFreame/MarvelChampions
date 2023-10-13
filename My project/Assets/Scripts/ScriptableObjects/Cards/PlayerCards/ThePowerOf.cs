using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "The Power Of", menuName = "MarvelChampions/Card Effects/The Power of Aspect")]
public class ThePowerOf : ResourceCardEffect
{
    public override List<Resource> GetResources()
    {
        if (PlayCardSystem.Instance.CardToPlay.CardAspect == Card.CardAspect)
        {
            return new List<Resource>() { Card.Resources[0], Card.Resources[0] };
        }

        return Card.Resources;
    }

    public override int ResourceCount(PlayerCard card)
    {
        if (card == null)
        {
            return 1;
        }

        if (card.CardAspect == Card.CardAspect)
        {
            return 2;
        }

        return 1;
    }
}
