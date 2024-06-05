using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Lockjaw", menuName = "MarvelChampions/Card Effects/Basic/Allies/Lockjaw")]
public class Lockjaw : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (Card.CurrZone != Zone.Discard && Card.CurrZone != Zone.Hand)
            return false;

        if (Card.CurrZone == Zone.Hand)
        {
            if (_owner.ResourcesAvailable(_card) < 4)
                return false;
        }
        else if (_card.CurrZone == Zone.Discard)
        {
            if (_owner.ResourcesAvailable() < 4)
                return false;
        }

        return true;
    }
}
