using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStatusAction : Action
{
    public CardUI owner;
    public Status status;
    public bool targetSelf;

    public ApplyStatusAction(Status status, bool targetSelf, CardUI owner = null) : base("ApplyStatusAction")
    {
        this.status = status;
        this.targetSelf = targetSelf;
        this.owner = owner;
    }

    public ApplyStatusAction(StatusData data) : base(data)
    {
        this.status = data.status;
        this.targetSelf = data.targetSelf;
    }

    public override void Execute() => ApplyStatusSystem.instance.InitiateStatus(this);
}
