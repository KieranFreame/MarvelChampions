using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrawCardSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static DrawCardSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    #region Fields
    private Player _player;
    [SerializeField] private Transform PlayerHand;
    #endregion

    public static UnityAction<PlayerCard> OnCardDrawn;

    public void DrawCards (DrawCardsAction action)
    {
        _player = (action.Drawer == null ? TurnManager.instance.CurrPlayer : action.Drawer);

        for (int i = 0; i < action.Value; i++)
        {
            if (_player.Deck.deck.Count == 0)
                _player.Deck.ResetDeck();

            CardData draw = _player.Deck.DealCard();

            PlayerCard inst = Instantiate(PrefabFactory.instance.CreatePlayerCard(draw as PlayerCardData), PlayerHand).GetComponent<PlayerCard>();
            inst.gameObject.name = draw.cardName;

            inst.LoadCardData(draw as PlayerCardData, _player);
            _player.Hand.AddToHand(inst);

            OnCardDrawn?.Invoke(inst);
        }
    }
}
