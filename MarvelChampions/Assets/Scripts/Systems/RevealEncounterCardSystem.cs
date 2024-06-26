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

    public EncounterCard CardToReveal { get; private set; }

    #region Methods
    public async Task InitiateRevealCard(EncounterCard cardToReveal)
    {
        CardToReveal = cardToReveal;
        Debug.Log("Revealing " + cardToReveal.CardName);
        await Task.Delay(3000);

        if (CardToReveal.CardType == CardType.Treachery || CardToReveal.CardType == CardType.Obligation)
            await EffectManager.Inst.AddEffect(cardToReveal, cardToReveal.Effect);

        if (CardToReveal == null) return;

        switch (CardToReveal.CardType)
        {
            case CardType.Treachery:
                ScenarioManager.inst.EncounterDeck.Discard(CardToReveal);
                break;
            case CardType.Obligation:
                if (!ScenarioManager.inst.RemovedFromGame.Contains(CardToReveal.Data))
                    ScenarioManager.inst.EncounterDeck.Discard(CardToReveal);
                break;
            default:
                if (CardToReveal.Effect != null)
                    await CardToReveal.Effect.OnEnterPlay();
                MoveCard(CardToReveal);
                break;
        }

        OnEncounterCardRevealed?.Invoke(CardToReveal);
        await EffectManager.Inst.CheckResponding();
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

    public void CancelCard()
    {
        ScenarioManager.inst.EncounterDeck.Discard(CardToReveal);
        CardToReveal = null;
    }
    #endregion
}
