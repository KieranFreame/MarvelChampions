using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakinAndTakin : SideScheme, IScheme
{
    [SerializeField]
    GameEvent whenRevealed;

    protected override void Start()
    {
        base.Start();
        whenRevealed.Raise();
    }

    public void WhenRevealed()
    {
        if (GameManager.instance.players == null) //temp
        {
            threat.GainThreat(1);
            return;
        }

        threat.GainThreat(1 * GameManager.instance.players.Count);
    }
}
