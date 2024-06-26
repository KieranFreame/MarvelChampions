using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Web-Shooter", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Web-Shooter")]
public class WebShooter : PlayerCardEffect, IGenerate
{
    Counters counters;

    public override Task OnEnterPlay()
    {
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        _owner.resourceGenerators.Add(CanGenerateResource);
        PayCostSystem.instance.GetAvailableResources += AvailableResources;

        return Task.CompletedTask;
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        counters.RemoveCounters(1);

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.resourceGenerators.Remove(CanGenerateResource);
            _owner.Deck.Discard(Card);
        }

        return new List<Resource>() { Resource.Wild };
    }

    public int CanGenerateResource()
    {
        return (!_card.Exhausted) ? 1 : 0;
    }

    public bool CompareResource(Resource resource)
    {
        return true;
    }

    public override void OnExitPlay()
    {
        _owner.resourceGenerators.Remove(CanGenerateResource);
        PayCostSystem.instance.GetAvailableResources -= AvailableResources;
    }

    public List<Resource> GetResources()
    {
       return new List<Resource>() { Resource.Wild };
    }

    public void AvailableResources()
    {
        PayCostSystem.instance.availableResources.Add(_card, GetResources());
    }
}
