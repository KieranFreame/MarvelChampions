using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Spider-Sense", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Enhanced Spider-Sense")]
public class EnhancedSpiderSense : PlayerCardEffect, ICancelEffect
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        base.OnDrawn(player, card);
        RevealEncounterCardSystem.instance.EffectCancelers.Add(this);
    }

    public override async Task OnEnterPlay()
    {
        RevealEncounterCardSystem.instance.EffectCancelers.Remove(this);
        await Task.Yield();
    }

    public async Task<bool> CancelEffect(ICard cardToCancel)
    {
        if ((cardToCancel as EncounterCard).Data.cardType is not CardType.Treachery)
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
