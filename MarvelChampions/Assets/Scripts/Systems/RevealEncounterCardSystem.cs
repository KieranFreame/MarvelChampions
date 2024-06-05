using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class RevealEncounterCardSystem
{
    private static RevealEncounterCardSystem instance;

    public static RevealEncounterCardSystem Instance
    {
        get
        {
            instance ??= new RevealEncounterCardSystem();
            return instance;
        }
    }

    private RevealEncounterCardSystem()
    {
        MinionTransform = GameObject.Find("MinionTransform").transform;
        SideSchemeTransform = GameObject.Find("SideSchemeTransform").transform;
        AttachmentTransform = GameObject.Find("AttachmentTransform").transform;

        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    #region Transforms
    public Transform MinionTransform { get; set; }
    public Transform SideSchemeTransform { get; set; }
    public Transform AttachmentTransform { get; set; }
    #endregion

    #region Events
    public static event UnityAction<EncounterCard> OnEncounterCardRevealed;
    #endregion

    public List<PlayerCardEffect> EffectCancelers { get; private set; } = new();
    public EncounterCard CardToReveal { get; private set; }

    #region Methods
    public async Task InitiateRevealCard(EncounterCard cardToReveal)
    {
        CardToReveal = cardToReveal;
        Debug.Log("Revealing " + cardToReveal.CardName);
        await Task.Delay(3000);

        await CheckCancellers();

        if (CardToReveal == null) //effect is cancelled
            return;

        if (CardToReveal.Data.cardType is not CardType.Treachery)
            MoveCard(CardToReveal);

        await CardToReveal.OnRevealCard();

        if (CardToReveal.Data.cardType is CardType.Treachery || (CardToReveal.Data.cardType == CardType.Obligation && ScenarioManager.inst.EncounterDeck.limbo.Contains(CardToReveal.Data)))
            ScenarioManager.inst.EncounterDeck.Discard(CardToReveal);

        OnEncounterCardRevealed?.Invoke(CardToReveal);
    }

    public void MoveCard(EncounterCard cardToReveal)
    {
        switch (cardToReveal.Data.cardType)
        {
            case CardType.Minion:
                cardToReveal.transform.parent.SetParent(MinionTransform);
                VillainTurnController.instance.MinionsInPlay.Add(cardToReveal as MinionCard);
                break;
            case CardType.SideScheme:
                cardToReveal.transform.parent.SetParent(SideSchemeTransform);
                ScenarioManager.sideSchemes.Add(cardToReveal as SchemeCard);
                break;
            case CardType.Attachment:
            case CardType.Environment:
                cardToReveal.transform.SetParent(AttachmentTransform);
                break;
        }
    }

    private async Task CheckCancellers()
    {
        for (int i = EffectCancelers.Count - 1; i >= 0; i--)
        {
            if (!EffectCancelers[i].CanResolve()) //if false; cannot resolve, move to next
                continue;

            if (EffectCancelers[i] is IOptional) //differentiate from a card like Adam Warlock's Cosmic Ward, which is a mandatory cancel
            {
                if (!await ConfirmActivateUI.MakeChoice(EffectCancelers[i].Card))
                    continue;
            }

            await EffectCancelers[i].Resolve();
            ScenarioManager.inst.EncounterDeck.Discard(CardToReveal);
            CardToReveal = null;

            break;
        }
    }
    #endregion
}
