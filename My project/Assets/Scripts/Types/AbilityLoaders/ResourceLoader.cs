using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : AbilityLoader
{
    public Resource resource { get; set; }
    public bool conditionMet { get; set; }

    public ResourceLoader(Card owner) : base(owner)
    {
        this.resource = (owner.data as PlayerCard).actionResource;
    }

    public override void TriggerAbility()
    {
        if (PayCostSystem.instance.resources.Contains(resource))
        {
            actions[1].Execute();
            return;
        }

        actions[0].Execute();
    }
}
