using System.Collections;
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

    public override bool CanActivate()
    {
        if (owner.Deck.deck.Count < 3)
            return false;

        return !hasActivated;
    }

    public override async void Activate()
    {
        hasActivated = true;

        List<CardData> data = owner.Deck.GetTop(3);
        owner.Deck.deck.RemoveRange(0, 3);
        owner.Deck.limbo.AddRange(data);

        List<ICard> cards = new();
        foreach (ICard c in CardViewerUI.inst.EnablePanel(data))
            cards.Add(c as PlayerCard);

        PlayerCard card = await TargetSystem.instance.SelectTarget(cards) as PlayerCard;
        cards.Remove(card);
        owner.Deck.Discard(cards);

        card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        owner.Hand.AddToHand(card);
    }
}
