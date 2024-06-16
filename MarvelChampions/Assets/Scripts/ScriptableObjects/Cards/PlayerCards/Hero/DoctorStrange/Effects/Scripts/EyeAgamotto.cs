using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Eye of Agamotto", menuName = "MarvelChampions/Card Effects/Doctor Strange/The Eye of Agamotto")]
public class EyeAgamotto : PlayerCardEffect, IGenerate
{
    public override Task OnEnterPlay()
    {
        _owner.resourceGenerators.Add(CanGenerateResource);
        return Task.CompletedTask;
    }

    public int CanGenerateResource()
    {
        if (_owner.Identity.ActiveIdentity is not Hero || _card.Exhausted)
            return 0;

        return 1;
    }

    public bool CompareResource(Resource resource)
    {
        return true; //generates Wild;
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        return new() { Resource.Wild };
    }
}
