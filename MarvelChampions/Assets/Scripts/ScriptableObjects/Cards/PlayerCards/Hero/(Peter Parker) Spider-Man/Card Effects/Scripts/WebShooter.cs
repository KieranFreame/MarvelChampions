using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Web-Shooter", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Web-Shooter")]
public class WebShooter : PlayerCardEffect, IGenerate
{
    Counters counters;

    public override async Task OnEnterPlay()
    {
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        counters.RemoveCounters(1);

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(Card);
        }

        return new List<Resource>() { Resource.Wild };
    }

    public bool CanGenerateResource(ICard cardToPlay)
    {
        return !_card.Exhausted;
    }

    public bool CompareResource(Resource resource)
    {
        return true; //Generates a Wild Resource, which matches everything;
    }
}
