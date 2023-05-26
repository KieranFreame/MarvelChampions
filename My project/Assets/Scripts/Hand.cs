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

    public bool Contains(PlayerCard card)
    {
        return cards.Contains(card);
    }

    public void Remove(PlayerCard card)
    {
        if (Contains(card))
            cards.Remove(card);
    }
}
