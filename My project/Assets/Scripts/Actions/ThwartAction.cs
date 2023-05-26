using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwartAction : Action
{
    public bool IgnoreCrisis { get; private set; } = false;

    public ThwartAction(int _thwart = 0, string requirement = "", bool ignoreCrisis = false)
    {
        Value = _thwart;
        Targets.Add(TargetType.TargetScheme);
        Requirement = requirement;
        IgnoreCrisis = ignoreCrisis;
    }
}
