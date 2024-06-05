using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Warning", menuName = "MarvelChampions/Card Effects/Basic/Warning")]
public class Warning : PlayerCardEffect, IOptional
{
    DamageAction action;

    public override void OnDrawn()
    {
        _owner.CharStats.Health.Modifiers.Add(ReduceDamage);
    }

    private async Task<DamageAction> ReduceDamage(DamageAction action)
    {
        if (await ConfirmActivateUI.MakeChoice(_card))
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
            action.Value--;
            _owner.CharStats.Health.Modifiers.Remove(ReduceDamage);
        }

        return action;
    }

    public override void OnDiscard()
    {
        _owner.CharStats.Health.Modifiers.Remove(ReduceDamage);
    }
}
