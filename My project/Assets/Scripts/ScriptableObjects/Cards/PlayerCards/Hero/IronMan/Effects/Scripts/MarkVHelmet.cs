using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mark V Helmet", menuName = "MarvelChampions/Card Effects/Iron Man/Mark V Helmet")]
public class MarkVHelmet : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;

        if (!FindObjectsOfType<Threat>().Any(x => x.CurrentThreat > 0))
            return false;

        if (Card.Exhausted)
            return false;

        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            if (_owner.CharStats.Thwarter.Confused)
            {
                _owner.CharStats.Thwarter.Confused = false;
                return;
            }

            foreach (Threat t in FindObjectsOfType<Threat>())
            {
                if (t.CurrentThreat > 0)
                    t.RemoveThreat(1);
            }
        }
        else
        {
            await _owner.CharStats.InitiateThwart(new(1));
        }
    }
}
