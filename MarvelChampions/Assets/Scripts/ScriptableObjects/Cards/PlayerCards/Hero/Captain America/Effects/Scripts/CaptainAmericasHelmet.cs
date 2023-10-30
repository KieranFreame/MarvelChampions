using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain America's Helmet", menuName = "MarvelChampions/Card Effects/Captain America/Captain America's Helmet")]
public class CaptainAmericasHelmet : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Health.Defeated.Add(Defeated);

        return Task.CompletedTask;
    }

    private Task Defeated()
    {
        _owner.CharStats.Health.CurrentHealth = 1;
        _owner.CharStats.Health.Defeated.Remove(Defeated);
        _owner.Deck.Discard(Card);
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Health.Defeated.Remove(Defeated);
    }
}
