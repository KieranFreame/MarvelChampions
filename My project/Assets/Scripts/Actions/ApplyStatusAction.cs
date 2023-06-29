using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStatusAction : Action
{
    public Status Status { get; private set; }

    public ApplyStatusAction(Status status, ICharacter owner = null, List<TargetType> targets = null)
    {
        Owner = owner;
        Targets = targets;
        Status = status;
    }
}
