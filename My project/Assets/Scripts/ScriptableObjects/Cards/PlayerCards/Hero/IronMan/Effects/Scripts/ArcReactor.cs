using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Arc Reactor", menuName = "MarvelChampions/Card Effects/Iron Man/Arc Reactor")]
public class ArcReactor : PlayerCardEffect
{
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

    public override async Task Activate()
    {
        Card.Exhaust();
        _owner.Ready();
        await Task.Yield();
    }
}
