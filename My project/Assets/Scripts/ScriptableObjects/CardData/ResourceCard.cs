using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Card", menuName = "Card/Resource")]
public class ResourceCard : PlayerCard
{
    public bool checkAspect;
    public bool doubleResource;

    public override List<Resource> GetResource(PlayerCard cardToPlay)
    {
        if (doubleResource)
        {
            if (checkAspect)
            {
                if (aspect.ToLower() != cardToPlay.aspect.ToLower())
                    return new List<Resource>() { resources[0] };
            }
        }

        return resources;
    }
}
