using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Great Responsibility", menuName = "MarvelChampions/Card Effects/Justice/Great Responsibility")]
public class GreatResponsibility : PlayerCardEffect, IModifyThreat
{
    SchemeAction _action;

    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        SchemeSystem.instance.Modifiers.Add(this);
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        player.CharStats.Health.TakeDamage(_action.Value);
        _action.Value = 0;
        SchemeSystem.instance.Modifiers.Remove(this);

        await Task.Yield();
    }

    public async Task<SchemeAction> ModifyScheme(SchemeAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            _action = action;
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
        }

        return action;
    }

    public override void OnDiscard()
    {
        SchemeSystem.instance.Modifiers.Remove(this);
    }
}
