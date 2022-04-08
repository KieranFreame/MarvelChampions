using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScheme : Encounter, IScheme
{
    public Threat threat { get; set; }

    protected virtual void Start()
    {
        threat = GetComponent<Threat>();
        threat.currThreat = threat.StartingThreat = (_cd as SchemeData).startingThreat;
    }

    protected virtual void WhenDefeated()
    {
        GameManager.instance.scenario.Discard(this);
    }
}
