using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Deck
{
    //dynamic owner;
    public List<CardData> deck;
    [SerializeField] private List<CardData> limbo;
    public List<CardData> discardPile;

    public event UnityAction OnDeckReset;

    public Deck()
    {
        //this.owner = owner;
        deck = new List<CardData>();
        limbo = new List<CardData>();
        discardPile = new List<CardData>();
    }
    public void ResetDeck()
    {
        Debug.Log("Resetting Deck");

        AddToDeck(discardPile);
        discardPile.Clear();
        Shuffle();

        OnDeckReset?.Invoke();
    }
    public List<CardData> GetTop (int amount)
    {
        List<CardData> topAmounts = new();

        for (int i = 0; i < amount; i++)
        {
            topAmounts.Add(deck[i]);
        }

        return topAmounts;
    }
    public void AddToDeck(CardData cardToAdd)
    {
        deck.Add(cardToAdd);
        Shuffle();
    }
    public void AddToDeck(List<CardData> cardToAdd)
    {
        deck.AddRange(cardToAdd);
        Shuffle();
    }
    public CardData DealCard()
    {
        CardData cardToDeal = deck[0];
        limbo.Add(cardToDeal);
        deck.Remove(cardToDeal);

        return cardToDeal;
    }
    public void Discard(Card discard)
    {
        CardData d = limbo.FirstOrDefault(x => x.cardName == discard.CardName);

        discardPile.Add(d);
        limbo.Remove(d);

        if (discard.gameObject != null) //hasn't already been destroyed
            UnityEngine.Object.Destroy(discard.gameObject);
    }
    public void Discard(List<Card> discards)
    {
        foreach (Card c in discards)
        {
            foreach (CardData data in limbo)
            {
                if (data.cardName == c.CardName)
                {
                    discardPile.Add(data);
                    limbo.Remove(data);
                    break;
                }
            }

            UnityEngine.Object.Destroy(c.gameObject);
        }
    }
    public void Shuffle()
    {
        //Fisher-Yates
        System.Random r = new();
        int n = deck.Count;

        while (n > 1)
        {
            int k = r.Next(n);
            n--;
            (deck[n], deck[k]) = (deck[k], deck[n]);
        }
    }
    public bool Contains(CardData data)
    {
        return (deck.Contains(data) || limbo.Contains(data) || discardPile.Contains(data));
    }
}
