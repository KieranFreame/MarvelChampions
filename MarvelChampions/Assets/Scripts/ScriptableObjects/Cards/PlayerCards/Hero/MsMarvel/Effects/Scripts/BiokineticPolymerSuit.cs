using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biokinetic Polymer Suit", menuName = "MarvelChampions/Card Effects/Ms Marvel/Biokinetic Polymer Suit")]
public class BiokineticPolymerSuit : PlayerCardEffect, IGenerate
{
    public bool CanGenerateResource(ICard cardToPlay)
    {
        if (Card.Exhausted)
            return false;

        if (cardToPlay.CardType != CardType.Event) 
            return false;

        return true;
    }

    public bool CompareResource(Resource resource)
    {
        return true; //produces a Wild resource
    }

    public List<Resource> GenerateResource()
    {
        Card.Exhaust();
        return new List<Resource>() { Resource.Wild };
    }
}
