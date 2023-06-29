using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Deck
{
    public List<CardData> deck;
    public List<CardData> limbo;
    public List<CardData> discardPile;

    public event UnityAction OnDeckReset;

    public Deck(string path)
    {
        deck = new List<CardData>();
        limbo = new List<CardData>();
        discardPile = new List<CardData>();

        AddToDeck(TextReader.PopulateDeck(path));
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
    public void Discard(ICard discard)
    {
        CardData d = limbo.FirstOrDefault(x => x.cardName == discard.CardName);

        if (d == null) return;

        discardPile.Add(d);
        limbo.Remove(d);

        if ((discard as MonoBehaviour).gameObject != null) //hasn't already been destroyed
            UnityEngine.Object.Destroy((discard as MonoBehaviour).gameObject);
    }
    public void Discard(List<ICard> discards)
    {
        foreach (ICard c in discards)
        {
            foreach (CardData data in limbo)
            {
                if (data == null)
                {
                    limbo.Remove(data);
                    continue;
                }

                if (data.cardName == c.CardName)
                {
                    discardPile.Add(data);
                    limbo.Remove(data);
                    break;
                }
            }

            UnityEngine.Object.Destroy((c as MonoBehaviour).gameObject);
        }
    }
    public void Mill(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            discardPile.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }
    private void Shuffle()
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
