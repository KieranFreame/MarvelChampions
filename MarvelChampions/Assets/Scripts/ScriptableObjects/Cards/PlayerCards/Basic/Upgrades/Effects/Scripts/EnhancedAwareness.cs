using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Awareness", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Enhanced Awareness")]
public class EnhancedAwareness : PlayerCardEffect, IGenerate
{
    Counters mental;

    public override Task OnEnterPlay()
    {
        mental = _card.gameObject.AddComponent<Counters>();
        mental.AddCounters(3);

        return Task.CompletedTask;
    }

    public bool CanGenerateResource(ICard cardToPlay)
    {
        return !_card.Exhausted;
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
}
