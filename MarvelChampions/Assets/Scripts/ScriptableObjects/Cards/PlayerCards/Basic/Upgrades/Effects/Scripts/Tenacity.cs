using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tenacity", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Tenacity")]
public class Tenacity : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return _owner.Exhausted && _owner.HaveResource(Resource.Physical);    
    }

    public override async Task Activate()
    {
        await PayCostSystem.instance.GetResources(resourceToCheck:Resource.Physical, amount: 1);

        _owner.Ready();

        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(_card);
    }
}
