using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

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

    public void AddToDeck(Card cardToAdd)
    {
        deck.Add(cardToAdd);
        //Shuffle();
    }
    
    public void AddToDeck(List<Card> cardToAdd)
    {
        deck.AddRange(cardToAdd);
        //Shuffle();
    }

    public Card Deal()
    {
        var cardToDeal = deck[0];
        deck.RemoveAt(0);
        return cardToDeal;
    }

    public void Discard(Card discard)
    {
        discardPile.Add(discard);
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
