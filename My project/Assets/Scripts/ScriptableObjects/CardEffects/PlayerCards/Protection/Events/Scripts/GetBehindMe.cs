using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Behind Me", menuName = "MarvelChampions/Card Effects/Protection/Get Behind Me")]
public class GetBehindMe : PlayerCardEffect, ICancelEffect
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        RevealEncounterCardSystem.instance.EffectCancelers.Add(this);
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        await FindObjectOfType<Villain>().CharStats.InitiateAttack();
        RevealEncounterCardSystem.instance.EffectCancelers.Remove(this);
    }

    public async Task<bool> CancelEffect(ICard cardToCancel)
    {
        var card = cardToCancel as EncounterCard;

        if (card.Data.cardType is not CardType.Treachery)
            return false;

        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            await PlayCardSystem.instance.InitiatePlayCard(new(_owner, _owner.Hand.cards, Card));
            return true;
        }

        return false;
    }

    public override void OnDiscard()
    {
        RevealEncounterCardSystem.instance.EffectCancelers.Remove(this);
    }
}
