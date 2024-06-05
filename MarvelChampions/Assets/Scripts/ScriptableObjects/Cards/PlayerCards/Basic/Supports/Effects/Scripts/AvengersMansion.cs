using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Avengers Mansion", menuName = "MarvelChampions/Card Effects/Basic/Supports/Avengers Mansion")]
public class AvengersMansion : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return !_card.Exhausted;
    }

    public override async Task Activate()
    {
        _card.Exhaust();

        DrawCardSystem.Instance.DrawCards(new(1, _owner));

        await Task.Yield();
    }
}
