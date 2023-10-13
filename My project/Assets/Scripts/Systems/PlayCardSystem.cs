using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayCardSystem
{
    private static PlayCardSystem instance;
    public static PlayCardSystem Instance
    {
        get
        {
            if (instance == null)
                instance = new();

            return instance;
        }
    }

    PlayCardSystem()
    {
        alliesTransform = GameObject.Find("AlliesTransform").transform;
        permanentsTransform = GameObject.Find("PermanentsTransform").transform;
        eventsTransform = GameObject.Find("EncounterCards").transform;
    }

    public PlayCardAction Action { get; private set; }
    public PlayerCard CardToPlay { get => Action.CardToPlay; }
    private Player _player;

    #region Events
    public event UnityAction<PlayerCard> OnCardPlayed;
    #endregion

    private Transform alliesTransform;
    private Transform permanentsTransform;
    private Transform eventsTransform;

    public async Task InitiatePlayCard(PlayCardAction action)
    {
        Action = action;
        _player = Action.Owner as Player;

        if (CardToPlay.CardCost > 0)
            await PayCostSystem.instance.PayForCard(CardToPlay);

        if (CardToPlay.CurrZone == Zone.Hand)
            _player.Hand.Remove(CardToPlay);

        OnCardPlayed?.Invoke(CardToPlay);

        await ChangeZones();
        await CardToPlay.OnEnterPlay();

        if (CardToPlay.CardType == CardType.Event && CardToPlay.InPlay)
            _player.Deck.Discard(CardToPlay);
    }

    private async Task ChangeZones()
    {
        CardToPlay.PrevZone = CardToPlay.CurrZone;
        CardToPlay.InPlay = true;

        if (CardToPlay.Data.cardType is CardType.Ally)
        {
            _player.CardsInPlay.Allies.Add(CardToPlay as AllyCard);

            if (_player.CardsInPlay.ReachedAllyLimit())
            {
                Debug.Log("Exceeded Ally Limit, Discard 1 Ally from Play");
                await DiscardAlly();
            }

            CardToPlay.transform.SetParent(alliesTransform);
            CardToPlay.CurrZone = Zone.Ally;
        }
        else if (CardToPlay.Data.cardType is CardType.Upgrade || CardToPlay.Data.cardType is CardType.Support)
        {
            CardToPlay.transform.SetParent(permanentsTransform);
            _player.CardsInPlay.Permanents.Add(CardToPlay);
            CardToPlay.CurrZone = Zone.Support;
        }
        else
        {
            CardToPlay.transform.SetParent(eventsTransform, false);
            CardToPlay.transform.localPosition = Vector3.zero;
            await Task.Delay(2000);
        }
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
