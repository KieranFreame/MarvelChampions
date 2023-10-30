using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Behind Me", menuName = "MarvelChampions/Card Effects/Protection/Events/Get Behind Me")]
public class GetBehindMe : PlayerCardEffect
{
    public override void OnDrawn()
    {
        RevealEncounterCardSystem.Instance.EffectCancelers.Add(CancelEffect);
    }

    public override async Task OnEnterPlay()
    {
        await FindObjectOfType<Villain>().CharStats.InitiateAttack();
        RevealEncounterCardSystem.Instance.EffectCancelers.Remove(CancelEffect);
    }

    public async Task<bool> CancelEffect(ICard cardToCancel)
    {
        var card = cardToCancel as EncounterCard;

        if (card.Data.cardType is not CardType.Treachery)
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
