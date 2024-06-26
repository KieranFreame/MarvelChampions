using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Superhuman Law Division", menuName = "MarvelChampions/Card Effects/She-Hulk/Superhuman Law Division")]
public class SuperhumanLawDivision : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not AlterEgo)
            return false;

        if (!FindObjectsOfType<SchemeCard>().Any(x => x.Threat.CurrentThreat > 0))
            return false;

        if (_card.Exhausted)
            return false;

        if (_owner.HaveResource(Resource.Scientific))
            return true;

        return false;
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(new() { { Resource.Scientific, 1 } });
        _card.Exhaust();

        await _owner.CharStats.InitiateThwart(new(2, null));
    }
}
