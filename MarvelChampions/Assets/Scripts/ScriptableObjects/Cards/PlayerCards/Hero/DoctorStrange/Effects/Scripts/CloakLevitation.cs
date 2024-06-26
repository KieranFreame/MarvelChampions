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

        if (_card.Exhausted)
            return false;

        if (!_owner.Exhausted)
            return false;

        return true;
    }

    public override Task OnEnterPlay()
    {
        _owner.Identity.IdentityTraits.AddItem("Aerial");
        return Task.CompletedTask;
    }

    public override Task Activate()
    {
        _card.Exhaust();
        _owner.Ready();
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        _owner.Identity.IdentityTraits.RemoveItem("Aerial");
    }
}
