using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwartAction : Action
{
    public SchemeCard Target { get; set; }
    public bool IgnoreCrisis { get; private set; } = false;

    public ThwartAction(int _thwart, bool ignoreCrisis = false)
    {
        Value = _thwart;
        Targets.Add(TargetType.TargetScheme);
        IgnoreCrisis = ignoreCrisis;
    }
}
