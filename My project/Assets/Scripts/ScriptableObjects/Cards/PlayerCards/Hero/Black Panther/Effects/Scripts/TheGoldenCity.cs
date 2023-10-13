using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Golden City", menuName = "MarvelChampions/Card Effects/Black Panther/The Golden City")]
public class TheGoldenCity : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not AlterEgo)
            return false;

        if (Card.Exhausted)
            return false;

        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        DrawCardSystem.Instance.DrawCards(new(2, _owner));
        await Task.Yield();
    }
}
