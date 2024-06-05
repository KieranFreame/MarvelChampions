using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Pepper Potts", menuName = "MarvelChampions/Card Effects/Iron Man/Pepper Potts")]
public class PepperPotts : PlayerCardEffect, IGenerate
{
    public bool CanGenerateResource(ICard cardToPlay)
    {
        if (_card.Exhausted)
            return false;

        if (_owner.Deck.discardPile.Count == 0)
            return false;

        return true;
    }

    public bool CompareResource(Resource resource)
    {
        if ((_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Contains(Resource.Wild))
            return true;

        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Contains(resource);
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources;
    }

    public int ResourceCount()
    {
        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Count;
    }
}
