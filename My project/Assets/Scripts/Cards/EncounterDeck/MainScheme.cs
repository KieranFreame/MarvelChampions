using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScheme : Card, IScheme
{
    public int acceleration { get; set; }
    public Threat threat { get; set; }

    public void Accelerate()
    {
        threat.GainThreat(acceleration);
    }

    public virtual void WhenCompleted(){ GameManager.instance.scenario.AdvanceMainScheme(); }
}
