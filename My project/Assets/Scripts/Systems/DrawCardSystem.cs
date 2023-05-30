using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DrawCards (DrawCardsAction action)
    {
        _player = (action.Drawer == null ? TurnManager.instance.CurrPlayer : action.Drawer);

        for (int i = 0; i < action.Value; i++)
        {
            CardData draw = _player.Deck.DealCard();

            GameObject inst = Instantiate(PrefabFactory.instance.CreatePlayerCard(draw as PlayerCardData), PlayerHand);
            inst.name = draw.cardName;

            inst.GetComponent<PlayerCard>().LoadCardData(draw, _player.gameObject);
            _player.Hand.cards.Add(inst.GetComponent<PlayerCard>());

            if (_player.Deck.deck.Count == 0)
                _player.Deck.ResetDeck();
        }
    }
}
