using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterParker : AlterEgo
{
    private void OnEnable()
    {
        idName = "Peter Parker";
        handSize = baseHandSize = 6;
        GetComponent<SpiderMan>().enabled = false;
        onFlip.Raise();
    }

    public override void SwitchIdentity()
    {
        GetComponent<SpiderMan>().enabled = true;
    }

    public override void Effect()
    {
        //Generate a Resource
    }
}
