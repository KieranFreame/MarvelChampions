using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter : Card, IBoost
{
    public int boostIcons { get; set; }

    public virtual void Effect() { }

    public virtual void BoostEffect() { return; }
}
