using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Peter Parker", menuName = "MarvelChampions/Identity Effects/Spider-Man (Peter Parker)/AlterEgo")]
public class PeterParker : IdentityEffect, IGenerate
{
    List<Resource> resource = new() { Resource.Scientific };

    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;

        owner.resourceGenerators.Add(CanGenerateResource);

        TurnManager.OnStartPlayerPhase += Reset;
    }

    public int CanGenerateResource()
    {
        return (!hasActivated) ? 1 : 0;
    }

    public List<Resource> GenerateResource()
    {
        hasActivated = true;
        return resource;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Scientific || resource == Resource.Wild;
    }

    public List<Resource> GetResources() => resource;
}
