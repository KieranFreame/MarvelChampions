using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Protective Ward", menuName = "MarvelChampions/Card Effects/Doctor Strange/Protective Ward")]
public class ProtectiveWard : PlayerCardEffect
{
    public override void OnDrawn()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Add(CancelEffect);
    }

    public override Task OnEnterPlay()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(CancelEffect);
        return Task.CompletedTask;
    }

    private async Task<bool> CancelEffect(EncounterCard cardToPlay)
    {
        if (cardToPlay.CardType != CardType.Treachery) return false;
        if (_owner.Identity.IdentityName != "Doctor Strange") return false;
        if (_owner.ResourcesAvailable(Card) < Card.CardCost) return false;

        bool activate = await ConfirmActivateUI.MakeChoice(Card);
        
        if (activate)
        {
            await PlayCardSystem.Instance.InitiatePlayCard(new(Card));
            return true;
        }

        return false;
    }

    public override void OnDiscard()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(CancelEffect);
    }
}
