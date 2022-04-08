using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMan : Hero
{
    private void OnEnable()
    {
        idName = "Spider-Man";
        handSize = baseHandSize = 5;
        GetComponent<PeterParker>().enabled = false;
    }

    public override void SwitchIdentity()
    {
        GetComponent<PeterParker>().enabled = true;
    }

    public void Effect()
    {
        /*
         * if (AttackSystem.Target == this)
         *      draw a card
         */
    }
}
