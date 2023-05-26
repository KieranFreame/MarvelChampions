using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResourceAction : Action
{
    public Resource resource;

    public Resource Execute(int filler = 0)
    {
        return resource;
    }
}
