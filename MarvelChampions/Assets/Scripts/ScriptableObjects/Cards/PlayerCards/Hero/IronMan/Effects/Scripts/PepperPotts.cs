using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Pepper Potts", menuName = "MarvelChampions/Card Effects/Iron Man/Pepper Potts")]
public class PepperPotts : PlayerCardEffect, IGenerate
{
    public override Task OnEnterPlay()
    {
        _owner.resourceGenerators.Add(CanGenerateResource);
        return Task.CompletedTask;
    }

    public int CanGenerateResource()
    {
        if (_card.Exhausted || _owner.Deck.discardPile.Count == 0)
            return 0;

        return ((PlayerCardData)_owner.Deck.discardPile.Last()).cardResources.Count;
    }

    public bool CompareResource(Resource resource)
    {
        if ((_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Contains(Resource.Wild))
            return true;

        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Contains(resource);
    }

    public List<Resource> GenerateResource()
    {
        _card.Exhaust();
        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources;
    }

    public int ResourceCount()
    {
        return (_owner.Deck.discardPile.Last() as PlayerCardData).cardResources.Count;
    }

    public override void OnExitPlay()
    {
        _owner.resourceGenerators.Remove(CanGenerateResource);
    }
}
