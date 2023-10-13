using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Steve's Apartment", menuName = "MarvelChampions/Card Effects/Captain America/Steve's Apartment")]
public class StevesApartment : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Steve Rogers")
            return false;

        if (Card.Exhausted)
            return false;

        return true;
    }

    public override Task Activate()
    {
        Card.Exhaust();

        DrawCardSystem.Instance.DrawCards(new(1, _owner));
        _owner.CharStats.Health.RecoverHealth(1);

        return Task.CompletedTask;
    }
}
