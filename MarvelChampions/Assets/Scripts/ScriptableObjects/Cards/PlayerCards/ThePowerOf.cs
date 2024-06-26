using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "The Power Of", menuName = "MarvelChampions/Card Effects/The Power of Aspect")]
public class ThePowerOf : ResourceCardEffect
{
    public override void GetResources()
    {
        PayCostSystem.instance.availableResources.Add(_card, 
            (PlayCardSystem.Instance.CardToPlay.CardAspect == _card.CardAspect) ? new List<Resource>() { _card.Resources[0], _card.Resources[0] } : new List<Resource>() { _card.Resources[0] });
    }

    public override int ResourceCount(PlayerCard card)
    {
        if (card == null || card.CardAspect != _card.CardAspect)
        {
            return 1;
        }

        return 2;
    }
}
