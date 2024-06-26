using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Reflexes", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Enhanced Reflexes")]
public class EnhancedReflexes : PlayerCardEffect, IGenerate
{
    Counters energy;
    List<Resource> resources = new List<Resource>() { Resource.Energy };

    public override Task OnEnterPlay()
    {
        energy = _card.gameObject.AddComponent<Counters>();
        energy.AddCounters(3);
        _owner.resourceGenerators.Add(CanGenerateResource);
        return Task.CompletedTask;
    }

    public int CanGenerateResource()
    {
        return (!_card.Exhausted) ? 1 : 0;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Energy;
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        energy.RemoveCounters(1);

        if (energy.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(_card);
        }

        return resources;
    }

    public List<Resource> GetResources() => resources;
}
