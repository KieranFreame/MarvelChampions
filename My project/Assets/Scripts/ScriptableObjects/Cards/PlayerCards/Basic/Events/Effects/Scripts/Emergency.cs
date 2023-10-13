using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Emergency", menuName = "MarvelChampions/Card Effects/Basic/Emergency")]
public class Emergency : PlayerCardEffect
{
    public override void OnDrawn()
    {
        SchemeSystem.Instance.Modifiers.Add(ModifyScheme);
    }

    public async Task<SchemeAction> ModifyScheme(SchemeAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));

            if (_owner.CharStats.Thwarter.Confused)
                _owner.CharStats.Thwarter.Confused = false;
            else
                action.Value--;
            
            SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
        }

        return action;
    }

    public override void OnDiscard()
    {
        SchemeSystem.Instance.Modifiers.Remove(ModifyScheme);
    }
}
