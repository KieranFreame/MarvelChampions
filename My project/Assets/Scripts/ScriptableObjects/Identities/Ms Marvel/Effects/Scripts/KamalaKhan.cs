using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kamala Khan", menuName = "MarvelChampions/Identity Effects/Ms Marvel/Alter-Ego")]
public class KamalaKhan : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
        hasActivated = false;
        TurnManager.OnEndPlayerPhase += Reset;
    }

    public override bool CanActivate()
    {
        return !hasActivated;
    }

    public override void Activate()
    {
        hasActivated = true;

        PlayerCardData data;

        do
        {
            data = owner.Deck.deck[0] as PlayerCardData;
            owner.Deck.Mill(1);
        } while (data.cardAspect != Aspect.Hero);

        PlayerCard card = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("PlayerHandTransform").transform) as PlayerCard;
        owner.Deck.discardPile.Remove(data);
        owner.Deck.limbo.Add(data);
        owner.Hand.AddToHand(card);
        card.Effect.OnDrawn();
    }
}
