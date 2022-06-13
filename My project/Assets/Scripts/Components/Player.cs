using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    public Identity identity;

    public Deck deck;
    public List<CardData> cardData = new List<CardData>();
    public List<Card> encounterCards = new List<Card>();

    public Hand hand = new Hand();

    public int allyLimit { get; set; }
    public List<Ally> activeAllies = new List<Ally>();

    public List<PlayerCard> upgrades = new List<PlayerCard>();
    public List<PlayerCard> supports = new List<PlayerCard>();
    public List<CardData> attachments = new List<CardData>();

    #endregion

    private void Start()
    {
        allyLimit = 3;
        deck = new Deck(this);

        cardData.AddRange(identity.heroSet.heroSet);

        for (int i = 0; i < cardData.Count; i++)
            deck.AddToDeck(new Card(cardData[i]));
    }
}
