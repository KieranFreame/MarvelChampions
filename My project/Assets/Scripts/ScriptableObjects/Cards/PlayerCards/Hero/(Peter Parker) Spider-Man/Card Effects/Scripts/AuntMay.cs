using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Aunt May", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Aunt May")]
public class AuntMay : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == Card.CardName))
            return false;

        return base.CanBePlayed();
    }

    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not AlterEgo || Card.Exhausted)
            return false;

        return _owner.CharStats.Health.Damaged();
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        _owner.CharStats.Health.RecoverHealth(4);

        await Task.Yield();
    }
}
