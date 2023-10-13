using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Focused Rage", menuName = "MarvelChampions/Card Effects/She-Hulk/Focused Rage")]
public class FocusedRage : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (Card.Exhausted)
            return false;

        return true;
    }

    public override async Task Activate()
    {
        _owner.CharStats.Health.CurrentHealth -= 1;
        Card.Exhaust();
        DrawCardSystem.Instance.DrawCards(new(1, _owner));
        await Task.Yield();
    }
}
