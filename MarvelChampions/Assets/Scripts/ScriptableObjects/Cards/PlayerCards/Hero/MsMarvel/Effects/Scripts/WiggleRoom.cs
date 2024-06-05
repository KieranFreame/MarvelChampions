using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Wiggle Room", menuName = "MarvelChampions/Card Effects/Ms Marvel/Wiggle Room")]
public class WiggleRoom : PlayerCardEffect
{
    public override void OnDrawn()
    {
        _owner.CharStats.Health.Modifiers.Add(ReduceDamage);
    }

    public override Task OnEnterPlay()
    {
        _owner.CharStats.Health.Modifiers.Remove(ReduceDamage);
        return Task.CompletedTask;
    }


    private async Task<DamageAction> ReduceDamage(DamageAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(_card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
            action.Value -= 3;
            DrawCardSystem.Instance.DrawCards(new(1, _owner));
        }

        return action;
    }
}
