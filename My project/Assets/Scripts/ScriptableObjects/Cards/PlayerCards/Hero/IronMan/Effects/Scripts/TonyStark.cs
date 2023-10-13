using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        for (int i = 0; i < 3; i++)
        { 
            owner.Deck.deck.RemoveAt(i);
        }

        owner.Deck.limbo.AddRange(data);

        List<ICard> cards = CardViewerUI.inst.EnablePanel(data);

        PlayerCard card = await TargetSystem.instance.SelectTarget(cards) as PlayerCard;
        cards.Remove(card);
        owner.Deck.Discard(cards);

        card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        owner.Hand.AddToHand(card);

        CardViewerUI.inst.DisablePanel();
    }
}
