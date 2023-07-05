using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Rocket Boots", menuName = "MarvelChampions/Card Effects/Iron Man/Rocket Boots")]
public class RocketBoots : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (Card.Exhausted)
            return false;

        if (_owner.Hand.cards.Any(x => x.Resources.Contains(Resource.Scientific) || x.Resources.Contains(Resource.Wild)))
            return true;

        foreach (PlayerCard c in _owner.CardsInPlay.Permanents)
        {
            if (c.Effect is IResourceGenerator)
                if ((c.Effect as IResourceGenerator).CompareResource(Resource.Scientific))
                    return true;
        }

        return false;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        await PayCostSystem.instance.GetResources(Resource.Scientific, 1);

        _owner.Identity.IdentityTraits.Add("Aerial");
        TurnManager.OnEndPlayerPhase += EndOfPhase;
    }

    private void EndOfPhase()
    {
        _owner.Identity.IdentityTraits.Remove("Aerial");
        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }
}
