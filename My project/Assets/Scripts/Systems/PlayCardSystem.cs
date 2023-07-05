using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayCardSystem : MonoBehaviour
{
    public static PlayCardSystem instance;
    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public PlayCardAction Action { get; private set; }
    public PlayerCard CardToPlay { get => Action.CardToPlay; }
    private Player _player;

    #region Events
    public event UnityAction<PlayerCard> OnCardPlayed;
    #endregion

    [SerializeField] private Transform alliesTransform;
    [SerializeField] private Transform permanentsTransform;

    public async Task InitiatePlayCard(PlayCardAction action)
    {
        Action = action;
        _player = Action.Owner as Player;

        if (CardToPlay.CardCost > 0)
            await PayCostSystem.instance.PayForCard(CardToPlay);

        _player.Hand.Remove(CardToPlay);

        await ChangeZones();
        await CardToPlay.OnEnterPlay();

        OnCardPlayed?.Invoke(CardToPlay);
    }

    private async Task ChangeZones()
    {
        if (CardToPlay.Data.cardType is CardType.Ally)
        {
            CardToPlay.transform.SetParent(alliesTransform);
            _player.CardsInPlay.Allies.Add(CardToPlay as AllyCard);

            if (_player.CardsInPlay.ReachedAllyLimit())
            {
                Debug.Log("Exceeded Ally Limit, Discard 1 Ally from Play");
                await DiscardAlly();
            }

            CardToPlay.PrevZone = CardToPlay.CurrZone;
            CardToPlay.CurrZone = Zone.Ally;
            CardToPlay.InPlay = true;
        }
        else if (CardToPlay.Data.cardType is CardType.Upgrade || CardToPlay.Data.cardType is CardType.Support)
        {
            CardToPlay.transform.SetParent(permanentsTransform);
            _player.CardsInPlay.Permanents.Add(CardToPlay);

            CardToPlay.PrevZone = CardToPlay.CurrZone;
            CardToPlay.CurrZone = Zone.Support;
            CardToPlay.InPlay = true;
        }
        else
            _player.Deck.Discard(CardToPlay);
        
    }
    public async Task DiscardAlly()
    {
        List<AllyCard> allies = _player.GetComponent<Player>().CardsInPlay.Allies.ToList();

        if (CardToPlay is AllyCard)
            allies.RemoveAll(x => x == CardToPlay as AllyCard);

        AllyCard target = await TargetSystem.instance.SelectTarget(allies);

        _player.Deck.Discard(target);
        _player.CardsInPlay.Allies.Remove(target);
    }
}
