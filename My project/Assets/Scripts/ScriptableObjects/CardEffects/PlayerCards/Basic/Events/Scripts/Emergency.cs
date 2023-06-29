using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Emergency", menuName = "MarvelChampions/Card Effects/Basic/Emergency")]
public class Emergency : PlayerCardEffect, IModifyThreat
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        SchemeSystem.instance.Modifiers.Add(this);
    }

    public async Task<SchemeAction> ModifyScheme(SchemeAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));

            if (_owner.CharStats.Thwarter.Confused)
                _owner.CharStats.Thwarter.Confused = false;
            else
                action.Value--;
            
            SchemeSystem.instance.Modifiers.Remove(this);
        }

        return action;
    }

    public override void OnDiscard()
    {
        SchemeSystem.instance.Modifiers.Remove(this);
    }
}
