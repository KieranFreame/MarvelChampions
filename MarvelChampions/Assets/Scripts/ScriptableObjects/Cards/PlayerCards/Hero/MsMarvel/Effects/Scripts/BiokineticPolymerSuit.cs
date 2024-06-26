using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Biokinetic Polymer Suit", menuName = "MarvelChampions/Card Effects/Ms Marvel/Biokinetic Polymer Suit")]
public class BiokineticPolymerSuit : PlayerCardEffect, IGenerate
{
    public override Task Resolve()
    {
        _owner.resourceGenerators.Add(CanGenerateResource);
        return Task.CompletedTask;
    }

    public int CanGenerateResource()
    {
        if (_card.Exhausted)
            return 0;

        return 1;
    }

    public bool CompareResource(Resource resource)
    {
        return true; //produces a Wild resource
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        return new() { Resource.Wild };
    }

    public List<Resource> GetResources() => new(){ Resource.Wild };
}
