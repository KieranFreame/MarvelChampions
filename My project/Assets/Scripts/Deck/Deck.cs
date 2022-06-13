using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    Player owner;
    public List<Card> deck;
    public List<Card> discardPile;

    public Deck(Player owner)
    {
        this.owner = owner;
        deck = new List<Card>();
        discardPile = new List<Card>();
    }

    public bool Update()
    {
        if (deck.Count == 0)
        {
            AddToDeck(discardPile);
            discardPile.Clear();
            Shuffle();
            return true;
        }

        return false;
    }

    public List<Card> GetTop (int amount)
    {
        List<Card> topAmounts = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            topAmounts.Add(deck[i]);
        }

        return topAmounts;
    }

    public void AddToDeck(Card cardToAdd)
    {
        deck.Add(cardToAdd);
        Shuffle();
    }
    
    public void AddToDeck(List<Card> cardToAdd)
    {
        deck.AddRange(cardToAdd);
        Shuffle();
    }

    public void Deal(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var cardToDeal = deck[0];
            deck.RemoveAt(0);
            owner.hand.cards.Add(cardToDeal);
        }  
    }

    public void Discard(Card discard)
    {
        discardPile.Add(discard);

        List<CardUI> cards = new List<CardUI>();
        GameObject gameObj = null;
        cards.AddRange(GameObject.FindObjectsOfType<CardUI>());

        foreach (CardUI card in cards)
        {
            if (card.card.data == discard.data)
                gameObj = card.gameObject;
        }

        GameObject.Destroy(gameObj);
    }
    public void Shuffle()
    {
        //Fisher-Yates
        System.Random r = new System.Random();
        int n = deck.Count;

        while (n > 1)
        {
            int k = r.Next(n);
            n--;
            Card temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }
}
