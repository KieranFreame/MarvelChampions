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
        return !_card.Exhausted && (_owner.HaveResource(Resource.Scientific) || _owner.HaveResource(Resource.Wild)) && _owner.Identity.ActiveIdentity is Hero;
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 1 } });
        _card.Exhaust();

        _owner.Identity.IdentityTraits.AddItem("Aerial");
        TurnManager.OnEndPlayerPhase += EndOfPhase;
    }

    private void EndOfPhase()
    {
        _owner.Identity.IdentityTraits.RemoveItem("Aerial");
        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }
}
