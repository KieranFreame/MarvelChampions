using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Warning", menuName = "MarvelChampions/Card Effects/Basic/Warning")]
public class Warning : PlayerCardEffect
{
    public override void OnDrawn()
    {
        DamageSystem.Instance.Modifiers.Add(ModifyDamage);
    }

    public async Task<DamageAction> ModifyDamage(DamageAction action)
    {
        if (!action.DamageTargets.Any(x => x is Player)) return action;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));

            action.Value--;
            
            DamageSystem.Instance.Modifiers.Remove(ModifyDamage);
        }

        return action;
    }

    public override void OnDiscard()
    {
        DamageSystem.Instance.Modifiers.Remove(ModifyDamage);
    }
}
