using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class DrawCardSystem
{
    private static DrawCardSystem instance;

    public static DrawCardSystem Instance
    {
        get
        {
            if (instance == null)
                instance = new();

            return instance;
        }
    }

    private DrawCardSystem()
    {
        PlayerHand = GameObject.Find("PlayerHandTransform").transform;
    }

    #region Fields
    private Player _player;
    [SerializeField] private Transform PlayerHand;
    #endregion

    public static UnityAction<PlayerCard> OnCardDrawn;

    public void DrawCards (DrawCardsAction action)
    {
        _player = action.Drawer == null ? TurnManager.instance.CurrPlayer : action.Drawer;

        for (int i = 0; i < action.Value; i++)
        {
            if (_player.Deck.deck.Count == 0)
                _player.Deck.ResetDeck();

            CardData draw = _player.Deck.DealCard();

            var inst = CreateCardFactory.Instance.CreateCard(draw, PlayerHand) as PlayerCard;
            _player.Hand.AddToHand(inst);
            OnCardDrawn?.Invoke(inst);
        }
    }
}
