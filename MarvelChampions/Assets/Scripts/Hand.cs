using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class Hand
{
    private readonly Player owner;
    public ObservableCollection<PlayerCard> cards;

    public Hand(Player owner)
    {
        cards = new();
        this.owner = owner;
    }

    public void AddToHand(PlayerCard card)
    {
        cards.Add(card);
        card.PrevZone = card.CurrZone;
        card.CurrZone = Zone.Hand;

        if (card.Effect != null)
            card.Effect.OnDrawn();

        PayCostSystem.instance.GetAvailableResources += card.GetResources;
    }

    public bool Contains(PlayerCard card)
    {
        return cards.Contains(card);
    }

    public void Discard(PlayerCard card)
    {
        if (cards.Contains(card))
        {
            if (card.Effect != null)
                card.Effect.OnDiscard();

            cards.Remove(card);
            owner.Deck.Discard(card);
        }
    }

    public void RemoveFromHand(PlayerCard card)
    {
        cards.Remove(card);
        PayCostSystem.instance.GetAvailableResources -= card.GetResources;
    }
}
