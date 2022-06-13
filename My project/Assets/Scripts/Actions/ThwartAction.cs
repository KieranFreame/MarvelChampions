using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwartAction : Action
{
    [HideInInspector]
    public Thwarter owner;
    public int thwart;

    public ThwartAction(ActionData data) : base (data){}

    public ThwartAction(int _thwart = 0, Thwarter owner = null) : base("ThwartAction")
    {
        this.owner = owner;

        if (this.owner == null)
            value = _thwart;
        else
            value = owner._thwart;
    }
    public override void Execute() => ThwartSystem.instance.InitiateThwart(this);
}
