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
    public int Amount { get; set; }
    public Player Player { get; set; }
    [SerializeField] private Transform PlayerHand;
    #endregion

    public void DrawCards (DrawCardsAction action)
    {
        Player = (action.Drawer == null ? TurnManager.instance.CurrPlayer : action.Drawer);

        Amount = (Player.Deck.deck.Count < action.Value ? Player.Deck.deck.Count :action.Value);

        for (int i = 0; i < Amount; i++)
        {
            //Spawns gameObject using prefab
            GameObject inst = Instantiate(PrefabFactory.instance.CreatePlayerCard(Player.Deck.deck[0] as PlayerCardData), PlayerHand);
            inst.name = Player.Deck.deck[0].cardName;

            inst.GetComponent<PlayerCard>().LoadCardData(Player.Deck.deck[0], Player.gameObject);
            Player.Hand.cards.Add(inst.GetComponent<PlayerCard>());

            Player.Deck.Deal(1);
        }

        if (Player.Deck.deck.Count == 0)
            Player.Deck.ResetDeck();
    }
    public Player GetPlayer
    {
        get { return Player; }
    }
}
