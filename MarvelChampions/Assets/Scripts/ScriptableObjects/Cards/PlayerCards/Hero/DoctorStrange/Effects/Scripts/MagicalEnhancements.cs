using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Magical Enhancements", menuName = "MarvelChampions/Card Effects/Doctor Strange/Magical Enhancements")]
public class MagicalEnhancements : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Thwarter.CurrentThwart++;
        _owner.CharStats.Defender.CurrentDefence++;

        TurnManager.OnStartPlayerPhase += StartOfPhase;
        return Task.CompletedTask;
    }

    private void StartOfPhase()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Thwarter.CurrentThwart--;
        _owner.CharStats.Defender.CurrentDefence--;

        TurnManager.OnStartPlayerPhase -= StartOfPhase;

        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Thwarter.CurrentThwart--;
        _owner.CharStats.Defender.CurrentDefence--;

        TurnManager.OnStartPlayerPhase -= StartOfPhase;
    }
}
