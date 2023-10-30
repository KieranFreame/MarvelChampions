using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Reflexes", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Enhanced Reflexes")]
public class EnhancedReflexes : PlayerCardEffect, IGenerate
{
    Counters energy;

    public override Task OnEnterPlay()
    {
        energy = Card.gameObject.AddComponent<Counters>();
        energy.AddCounters(3);

        return Task.CompletedTask;
    }

    public bool CanGenerateResource(ICard cardToPlay)
    {
        return !Card.Exhausted;
    }

    public bool CompareResource(Resource resource)
    {
        return resource == Resource.Energy;
    }

    public List<Resource> GenerateResource()
    {
        Card.Exhaust();
        energy.RemoveCounters(1);

        if (energy.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
        }

        return new List<Resource>() { Resource.Energy };
    }
}
