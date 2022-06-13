using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchemeAction : Action
{
    public Schemer owner;
    public int scheme;

    public SchemeAction(int scheme = 0, Schemer owner = null) : base("SchemeAction")
    {
        this.owner = owner;

        if (this.owner != null)
            value = owner._scheme;
        else
            value = scheme;
    }

    public SchemeAction(ActionData data) : base(data) { }

    public override void Execute() => SchemeSystem.instance.InitiateScheme(this);
}
