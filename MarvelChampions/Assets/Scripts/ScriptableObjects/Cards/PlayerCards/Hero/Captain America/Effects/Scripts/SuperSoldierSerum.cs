using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Super-Soldier Serum", menuName = "MarvelChampions/Card Effects/Captain America/Super-Soldier Serum")]
public class SuperSoldierSerum : PlayerCardEffect, IGenerate
{
    public bool CanGenerateResource(ICard cardToPlay)
    {
        return !Card.Exhausted;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Physical;
    }

    public List<Resource> GenerateResource()
    {
        Card.Exhaust();
        return new List<Resource>() { Resource.Physical };
    }
}
