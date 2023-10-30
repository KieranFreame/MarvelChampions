using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Spider-Sense", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Enhanced Spider-Sense")]
public class EnhancedSpiderSense : PlayerCardEffect
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

    public async Task<bool> CancelEffect(ICard cardToCancel)
    {
        if ((cardToCancel as EncounterCard).Data.cardType is not CardType.Treachery)
            return false;
        
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
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
