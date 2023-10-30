using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Powered Gauntlets", menuName = "MarvelChampions/Card Effects/Iron Man/Powered Gauntlets")]
public class PoweredGauntlets : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (Card.Exhausted)
            return false;

        if (FindObjectOfType<Villain>() == null && VillainTurnController.instance.MinionsInPlay.Count == 0)
            return false;
           
        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
            await _owner.CharStats.InitiateAttack(new(2, owner: _owner, card: Card));
        else
            await _owner.CharStats.InitiateAttack(new(1, owner: _owner, card: Card));
    }
}
