using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsAction : Action
{
    public Player Drawer { get; private set; }

    public DrawCardsAction(int amount, Player drawer = null) : base ()
    {
        this.Drawer = drawer;
        Value = amount;
    }
}
