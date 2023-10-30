using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Master of the Mystic Arts", menuName = "MarvelChampions/Card Effects/Doctor Strange/Master of the Mystic Arts")]
public class MasterMysticArts : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.IdentityName != "Doctor Strange")
                return false;

            if (_owner.ResourcesAvailable() < InvocationDeck.Instance.Invocations[0].cardCost)
                return false;

            return InvocationDeck.Instance.CanActivate();
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        await PayCostSystem.instance.GetResources(amount: InvocationDeck.Instance.Invocations[0].cardCost);
        await InvocationDeck.Instance.Activate();
    }
}
