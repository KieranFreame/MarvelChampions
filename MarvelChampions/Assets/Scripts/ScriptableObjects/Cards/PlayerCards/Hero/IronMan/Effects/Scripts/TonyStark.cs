using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tony Stark", menuName = "MarvelChampions/Identity Effects/Iron Man/Alter Ego")]
public class TonyStark : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;

        hasActivated = false;
        TurnManager.OnEndPlayerPhase += Reset;
    }

    public override bool CanActivate() => !hasActivated;

    public override async void Activate()
    {
        hasActivated = true;
        int cardsToGet = Math.Min(owner.Deck.deck.Count, 3);

        List<CardData> data = new();
        
        for (int i = 0; i < cardsToGet; i++)
        {
            data.Add(owner.Deck.DealCard());
        }

        List<ICard> cards = CardViewerUI.inst.EnablePanel(data);

        PlayerCard card = await TargetSystem.instance.SelectTarget(cards) as PlayerCard;
        cards.Remove(card);
        owner.Deck.Discard(cards);

        card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        owner.Hand.AddToHand(card);

        CardViewerUI.inst.DisablePanel();
    }
}
