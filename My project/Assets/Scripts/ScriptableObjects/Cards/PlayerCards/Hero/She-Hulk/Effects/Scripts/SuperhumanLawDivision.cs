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

        if (!FindObjectsOfType<Threat>().Any(x => x.CurrentThreat > 0))
            return false;

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
        await PayCostSystem.instance.GetResources(Resource.Scientific, 1);
        Card.Exhaust();

        await _owner.CharStats.InitiateThwart(new(2));
    }
}
