using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "The Power Of", menuName = "MarvelChampions/Card Effects/The Power of Aspect")]
public class ThePowerOf : ResourceCardEffect
{
    public override List<Resource> GetResources()
    {
        if (PlayCardSystem.Instance.CardToPlay.CardAspect == _card.CardAspect)
        {
            return new List<Resource>() { _card.Resources[0], _card.Resources[0] };
        }

        return _card.Resources;
    }

    public override int ResourceCount(PlayerCard card)
    {
        if (card == null)
        {
            return 1;
        }

        if (card.CardAspect == _card.CardAspect)
        {
            return 2;
        }

        return 1;
    }
}
