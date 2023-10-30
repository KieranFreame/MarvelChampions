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
            if (instance == null)
                instance = new RevealEncounterCardSystem();

            return instance;
        }
    }

    private RevealEncounterCardSystem()
    {
        _minionTransform = GameObject.Find("MinionTransform").transform;
        _sideSchemeTransform = GameObject.Find("SideSchemeTransform").transform;
        _attachmentTransform = GameObject.Find("AttachmentTransform").transform;

        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    #region Transforms
    [SerializeField] private Transform _minionTransform;
    [SerializeField] private Transform _sideSchemeTransform;
    [SerializeField] private Transform _attachmentTransform;
    #endregion

    #region Events
    public static event UnityAction<EncounterCard> OnEncounterCardRevealed;
    #endregion

    #region Delegates
    public delegate Task<bool> CancelEffect(EncounterCard cardToPlay);
    public List<CancelEffect> EffectCancelers { get; private set; } = new();
    #endregion

    private bool canceled = false;

    #region Methods
    public async Task InitiateRevealCard(EncounterCard cardToPlay)
    {
        Debug.Log("Revealing " + cardToPlay.CardName);
        await Task.Delay(3000);

        if (cardToPlay.Data.cardType is not CardType.Treachery)
            MoveCard(cardToPlay);

        canceled = false;

        for (int i = EffectCancelers.Count - 1; i >= 0; i--)
        {
            canceled = await EffectCancelers[i](cardToPlay);

            if (canceled)
                break;
        }

        if (!canceled)
            await cardToPlay.OnRevealCard();

        if (cardToPlay.Data.cardType is CardType.Treachery || (cardToPlay.Data.cardType == CardType.Obligation && ScenarioManager.inst.EncounterDeck.limbo.Contains(cardToPlay.Data)))
            ScenarioManager.inst.EncounterDeck.Discard(cardToPlay);

        OnEncounterCardRevealed?.Invoke(cardToPlay);
    }

    public void MoveCard(EncounterCard cardToPlay)
    {
        switch (cardToPlay.Data.cardType)
        {
            case CardType.Minion:
                cardToPlay.transform.SetParent(_minionTransform);
                VillainTurnController.instance.MinionsInPlay.Add(cardToPlay as MinionCard);
                break;
            case CardType.SideScheme:
                cardToPlay.transform.parent.SetParent(_sideSchemeTransform);
                ScenarioManager.sideSchemes.Add(cardToPlay as SchemeCard);
                break;
            case CardType.Attachment:
            case CardType.Environment:
                cardToPlay.transform.SetParent(_attachmentTransform);
                break;
        }
    }
    #endregion
}
