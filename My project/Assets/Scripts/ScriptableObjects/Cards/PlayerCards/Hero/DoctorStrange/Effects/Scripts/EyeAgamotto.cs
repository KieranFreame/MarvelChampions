using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "The Eye of Agamotto", menuName = "MarvelChampions/Card Effects/Doctor Strange/The Eye of Agamotto")]
public class EyeAgamotto : PlayerCardEffect, IGenerate
{
    public bool CanGenerateResource(ICard cardToPlay)
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;
        if (Card.Exhausted)
            return false;

        return true;
    }

    public bool CompareResource(Resource resource)
    {
        return true; //generates Wild;
    }

    public List<Resource> GenerateResource()
    {
        Card.Exhaust();
        return new() { Resource.Wild };
    }
}
