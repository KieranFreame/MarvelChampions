using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCard : PlayerCard
{
    public override void GetResources()
    {
        if (Effect != null)
        {
            (Effect as ResourceCardEffect).GetResources();
            return;
        }

        base.GetResources();
    }

    public int ResourceCount(PlayerCard c)
    {
        if (Effect != null)
            return (Effect as ResourceCardEffect).ResourceCount(c);

        return Resources.Count;
    }


}
