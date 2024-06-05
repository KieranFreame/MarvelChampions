using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Iron Fist", menuName = "MarvelChampions/Card Effects/Protection/Allies/Iron Fist")]
public class IronFist : PlayerCardEffect
{
    Counters mystic;

    public override Task OnEnterPlay()
    {
        mystic = _card.gameObject.AddComponent<Counters>();
        mystic.AddCounters(2);

        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        DamageSystem.Instance.Modifiers.Add(ApplyStun);
    }

    private async Task<DamageAction> ApplyStun(DamageAction action)
    {
        DamageSystem.Instance.Modifiers.Remove(ApplyStun);

        if (await ConfirmActivateUI.MakeChoice(Card))
        {
            mystic.RemoveCounters(1);
            action.DamageTargets[0].CharStats.Attacker.Stunned = true;
            action.DamageTargets[0].CharStats.Health.TakeDamage(new(action.DamageTargets[0], 1));

            if (mystic.CountersLeft == 0)
                (Card as AllyCard).CharStats.AttackInitiated -= AttackInitiated;
        }

        return action;
    }
}
