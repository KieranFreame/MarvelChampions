using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCard : PlayerCard
{
    public List<Resource> GetResources()
    {
        if (Effect != null)
            return (Effect as ResourceCardEffect).GetResources();

        return Resources;
    }
}
