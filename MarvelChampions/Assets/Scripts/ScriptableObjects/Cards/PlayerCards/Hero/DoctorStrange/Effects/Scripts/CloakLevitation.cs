using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Cloak of Levitation", menuName = "MarvelChampions/Card Effects/Doctor Strange/The Cloak of Levitation")]
public class CloakLevitation : PlayerCardEffect
{
    bool addedTrait;

    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;

        if (Card.Exhausted)
            return false;

        if (!_owner.Exhausted)
            return false;

        return true;
    }

    public override Task OnEnterPlay()
    {
        if (!_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            _owner.Identity.IdentityTraits.Add("Aerial");
            addedTrait = true;
        }

        return Task.CompletedTask;
    }

    public override Task Activate()
    {
        Card.Exhaust();
        _owner.Ready();
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        if (addedTrait)
        {
            _owner.Identity.IdentityTraits.Remove("Aerial");
        }
    }
}
