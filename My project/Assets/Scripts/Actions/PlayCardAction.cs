using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : Action
{
    public Player owner;
    public List<Card> candidates = new List<Card>();
    public Card cardToPlay;

    public PlayCardAction(Player owner, List<Card> candidates, Card cardToPlay) : base ("PlayCardAction")
    {
        this.owner = owner;
        this.candidates = candidates;
        this.cardToPlay = cardToPlay;
    }

    public PlayCardAction(ActionData data) : base(data) { }

    public override void Execute() => PlayCardSystem.instance.InitiatePlayCard(this);
}
