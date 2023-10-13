using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class Hand
{
    public ObservableCollection<PlayerCard> cards;

    public Hand()
    {
        cards = new();
    }

    public void AddToHand(PlayerCard card)
    {
        cards.Add(card);
        card.PrevZone = card.CurrZone;
        card.CurrZone = Zone.Hand;
        card.Effect?.OnDrawn();
    }

    public bool Contains(PlayerCard card)
    {
        return cards.Contains(card);
    }

    public void Remove(PlayerCard card)
    {
        if (cards.Contains(card))
        {
            card.Effect?.OnDiscard();
            cards.Remove(card);
        }
    }
}
