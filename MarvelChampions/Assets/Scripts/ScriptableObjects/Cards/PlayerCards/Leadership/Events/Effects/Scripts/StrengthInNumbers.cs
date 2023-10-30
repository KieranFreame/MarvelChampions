using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Strength In Numbers", menuName = "MarvelChampions/Card Effects/Leadership/Strength In Numbers")]
public class StrengthInNumbers : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.CardsInPlay.Allies.Count == 0) return false;
            if (_owner.CardsInPlay.Allies.Where(x => !x.Exhausted).Count() == 0) return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        CancellationToken token = FinishButton.ToggleFinishButton(true, FinishedSelection);
        List<AllyCard> allies = await TargetSystem.instance.SelectTargets(_owner.CardsInPlay.Allies.Where(x => !x.Exhausted).ToList(), _owner.CardsInPlay.Allies.Where(x => !x.Exhausted).Count(), token);
        FinishButton.ToggleFinishButton(false, FinishedSelection);

        foreach (var a in allies)
        {
            a.Exhaust();
            DrawCardSystem.Instance.DrawCards(new(1, _owner));
        }
    }

    private void FinishedSelection()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelection);
    }
}
