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

        if (!FindObjectsOfType<SchemeCard>().Any(x => x.Threat.CurrentThreat > 0))
            return false;

        if (_card.Exhausted)
            return false;

        return true;
    }

    public override async Task Activate()
    {
        _card.Exhaust();

        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            if (_owner.CharStats.Thwarter.Confused)
            {
                _owner.CharStats.Thwarter.Confused = false;
                return;
            }

            foreach (SchemeCard s in FindObjectsOfType<SchemeCard>())
            {
                if (s.Threat.CurrentThreat > 0)
                    s.Threat.RemoveThreat(1);
            }
        }
        else
        {
            await _owner.CharStats.InitiateThwart(new(1, Owner));
        }
    }
}
