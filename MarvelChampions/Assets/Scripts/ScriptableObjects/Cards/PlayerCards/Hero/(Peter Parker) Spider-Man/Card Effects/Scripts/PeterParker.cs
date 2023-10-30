using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Peter Parker", menuName = "MarvelChampions/Identity Effects/Spider-Man (Peter Parker)/AlterEgo")]
public class PeterParker : IdentityEffect, IGenerate
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;

        TurnManager.OnStartPlayerPhase += Reset;
    }

    public bool CanGenerateResource(ICard cardToPlay)
    {
        return !hasActivated;
    }

    public List<Resource> GenerateResource()
    {
        hasActivated = true;
        return new List<Resource>() { Resource.Scientific };
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Scientific;
    }
}
