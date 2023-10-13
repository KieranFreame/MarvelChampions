using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Fearless Determination", menuName = "MarvelChampions/Card Effects/Captain America/Fearless Determination")]
public class FearlessDetermination : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Thwarter.CurrentThwart++;
        DrawCardSystem.Instance.DrawCards(new(1, _owner));

        TurnManager.OnEndPlayerPhase += OnEndPhase;
        return Task.CompletedTask;
    }

    private void OnEndPhase()
    {
        _owner.CharStats.Thwarter.CurrentThwart--;
    }
}
