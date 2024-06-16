using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Super-Soldier Serum", menuName = "MarvelChampions/Card Effects/Captain America/Super-Soldier Serum")]
public class SuperSoldierSerum : PlayerCardEffect, IGenerate
{
    public int CanGenerateResource()
    {
        return (!_card.Exhausted) ? 1 : 0;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Physical;
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        return new List<Resource>() { Resource.Physical };
    }
}
