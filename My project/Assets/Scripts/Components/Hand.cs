using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand
{
    public List<Card> cards;

    public Hand()
    {
        cards = new List<Card>();
    }

    public bool Contains(Card card)
    {
        foreach (Card c in cards)
        {
            if (c.data == card.data)
                return true;
        }

        return false;
    }

    public void Remove(Card card)
    {
        foreach (Card c in cards)
        {
            if (c.data == card.data)
            {
                cards.Remove(c);
                break;
            }
                
        }
    }
}
