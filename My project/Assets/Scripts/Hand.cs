using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand
{
    public List<PlayerCard> cards;

    public Hand()
    {
        cards = new List<PlayerCard>();
    }

    public void AddToHand(PlayerCard card)
    {
        cards.Add(card);
        card.Data.effect?.OnDrawn(card.Owner, card);
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
