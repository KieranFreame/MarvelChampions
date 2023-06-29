using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class RevealEncounterCardSystem : MonoBehaviour
{
    public static RevealEncounterCardSystem instance;

    private void Awake()
    {
        //Singleton
        if (instance == null) { instance = this; }
        else { Destroy(this); }
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
    public List<ICancelEffect> EffectCancelers { get; private set; } = new();
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
            canceled = await EffectCancelers[i].CancelEffect(cardToPlay);

            if (canceled)
                break;
        }

        if (!canceled)
            await cardToPlay.OnRevealCard();

        if (cardToPlay.Data.cardType is CardType.Treachery || (cardToPlay.Data.cardType == CardType.Obligation && ScenarioManager.inst.EncounterDeck.Contains(cardToPlay.Data)))
            ScenarioManager.inst.EncounterDeck.Discard(cardToPlay);

        OnEncounterCardRevealed?.Invoke(cardToPlay);
    }

    private void MoveCard(EncounterCard cardToPlay)
    {
        switch (cardToPlay.Data.cardType)
        {
            case CardType.Minion:
                cardToPlay.transform.SetParent(_minionTransform);
                VillainTurnController.instance.MinionsInPlay.Add(cardToPlay as MinionCard);
                break;
            case CardType.SideScheme:
                cardToPlay.transform.SetParent(_sideSchemeTransform);
                ScenarioManager.sideSchemes.Add(cardToPlay as SchemeCard);
                break;
            case CardType.Attachment:
                cardToPlay.transform.SetParent(_attachmentTransform);
                break;
        }
    }
    #endregion
}
