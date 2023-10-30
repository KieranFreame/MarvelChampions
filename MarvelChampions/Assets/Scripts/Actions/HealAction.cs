using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : Action
{
    public Health Target { get; private set; }

    public HealAction(int value, Health target)
    {
        Value = value;
        Target = target;
        
    }

    public HealAction(int value, TargetType targetType)
    {
        Value = value;
        Targets.Add(targetType);
    }
}
