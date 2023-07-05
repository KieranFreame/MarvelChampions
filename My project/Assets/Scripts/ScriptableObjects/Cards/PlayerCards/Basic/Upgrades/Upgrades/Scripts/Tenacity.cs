using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tenacity", menuName = "MarvelChampions/Card Effects/Basic/Tenacity")]
public class Tenacity : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (!_owner.Exhausted)
            return false;

        if (_owner.Hand.cards.Any(x => x.Resources.Contains(Resource.Physical) || x.Resources.Contains(Resource.Wild)))
            return true;
        
        foreach (PlayerCard c in _owner.CardsInPlay.Permanents)
        {
            if (c.Effect is IResourceGenerator)
                if ((c.Effect as IResourceGenerator).CompareResource(Resource.Physical))
                    return true;
        }

        return false;
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(resourceToCheck:Resource.Physical, amount: 1);

        _owner.Ready();

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }
}
