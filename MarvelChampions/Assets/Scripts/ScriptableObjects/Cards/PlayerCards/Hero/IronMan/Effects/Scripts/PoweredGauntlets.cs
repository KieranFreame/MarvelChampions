using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Powered Gauntlets", menuName = "MarvelChampions/Card Effects/Iron Man/Powered Gauntlets")]
public class PoweredGauntlets : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_card.Exhausted)
            return false;

        if (FindObjectOfType<Villain>() == null && VillainTurnController.instance.MinionsInPlay.Count == 0)
            return false;
           
        return true;
    }

    public override async Task Activate()
    {
        _card.Exhaust();
        await _owner.CharStats.InitiateAttack(new(_owner.Identity.IdentityTraits.Contains("Aerial") ? 2 : 1, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner, card: Card));
    }
}
