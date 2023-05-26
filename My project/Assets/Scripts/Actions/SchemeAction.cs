using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeAction : Action
{
    public SchemeAction(int scheme, GameObject owner)
    {
        Owner = owner;
        Value = scheme;
        Targets.Add(TargetType.TargetScheme);
    }
}
