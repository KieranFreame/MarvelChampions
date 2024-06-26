using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Awareness", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Enhanced Awareness")]
public class EnhancedAwareness : PlayerCardEffect, IGenerate
{
    Counters mental;

    public override Task Resolve()
    {
        mental = _card.gameObject.AddComponent<Counters>();
        mental.AddCounters(3);
        _owner.resourceGenerators.Add(CanGenerateResource);

        return Task.CompletedTask;
    }

    public int CanGenerateResource()
    {
        return (!_card.Exhausted) ? 1 : 0;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Scientific;
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        mental.RemoveCounters(1);

        if (mental.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(_card);
        }

        return new List<Resource>() { Resource.Scientific };
    }

    public List<Resource> GetResources()
    {
        return new() { Resource.Scientific };
    }
}
