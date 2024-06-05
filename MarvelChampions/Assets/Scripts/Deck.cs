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

    private readonly DeckUI deckUI;

    public event UnityAction<CardData> DeckChanged;
    public event UnityAction DiscardChanged;
    public event UnityAction OnDeckReset;

    public Deck(List<CardData> cards)
    {
        deck = new();
        limbo = new List<CardData>();
        discardPile = new List<CardData>();

        deckUI = Object.FindObjectOfType<DeckUI>();

        AddToDeck(cards);
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

        DeckChanged?.Invoke(cardToAdd);
    }
    public void AddToDeck(List<CardData> cardsToAdd)
    {
        foreach (var data in cardsToAdd)
            AddToDeck(data);
    }
    public CardData DealCard()
    {
        if (deck.Count == 0)
            ResetDeck();

        CardData cardToDeal = deck[0];
        limbo.Add(cardToDeal);
        deck.Remove(cardToDeal);

        DeckChanged?.Invoke(null);

        return cardToDeal;
    }
    public void Discard(ICard discard)
    {
        CardData d = limbo.FirstOrDefault(x => x.cardName == discard.CardName);

        if (d == default) return;

        discardPile.Add(d);
        limbo.Remove(d);

        if ((discard as MonoBehaviour).gameObject != null)
            if (discard is SchemeCard || discard is MinionCard)
                Object.Destroy((discard as MonoBehaviour).transform.parent.gameObject);
            else
                Object.Destroy((discard as MonoBehaviour).gameObject);

        DiscardChanged?.Invoke();
    }
    public void Discard(List<ICard> discards)
    {
        foreach (ICard c in discards)
        {
            Discard(c);
        }
    }
    public void Mill(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (deck.Count == 0)
            {
                ResetDeck();
                break;
            }

            ICard card = CreateCardFactory.Instance.CreateCard(deck[0], null);

            limbo.Add(deck[0]);
            deck.RemoveAt(0);

            Discard(card);
        }

        DeckChanged?.Invoke(null);
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
        return (deck.Contains(data) || discardPile.Contains(data));
    }
    public CardData Search(string cardName, bool searchDiscard = false)
    {
        if (limbo.Find(x => x.cardName == cardName) != null)
            return null;

        CardData data = deck.FirstOrDefault(x => x.cardName == cardName);

        if (data != null)
        {
            deck.Remove(data);
            limbo.Add(data);
        }
        else
        {
            if (searchDiscard)
            {
                data = discardPile.FirstOrDefault(x => x.cardName == cardName);

                if (data != null)
                {
                    discardPile.Remove(data);
                    limbo.Add(data);
                }
            }
        }

        Shuffle();

        DeckChanged?.Invoke(data);

        return data;
    }       
}
