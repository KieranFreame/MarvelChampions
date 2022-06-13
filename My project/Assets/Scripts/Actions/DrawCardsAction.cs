using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsAction : Action
{
    public Player drawer;

    public DrawCardsAction(ActionData data) : base(data) { }

    public DrawCardsAction(int amount, Player drawer = null) : base ("DrawCardsAction", amount)
    {
        this.drawer = drawer;
    }

    public override void Execute() => DrawCardSystem.instance.DrawCards(this);
}
