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

        if (!_owner.HaveResource(Resource.Scientific))
            return false;

        return true;
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(Resource.Scientific, 1);
        Card.Exhaust();

        _owner.Identity.IdentityTraits.Add("Aerial");
        TurnManager.OnEndPlayerPhase += EndOfPhase;
    }

    private void EndOfPhase()
    {
        _owner.Identity.IdentityTraits.Remove("Aerial");
        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }
}
