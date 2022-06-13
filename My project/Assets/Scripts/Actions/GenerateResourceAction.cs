using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResourceAction : Action
{
    public Resource resource;

    public GenerateResourceAction(ActionData data) : base(data)
    {
        resource = (Resource)data.value;
    }

    public override void Execute() { }

    public Resource Execute(int filler = 0)
    {
        return resource;
    }
}
