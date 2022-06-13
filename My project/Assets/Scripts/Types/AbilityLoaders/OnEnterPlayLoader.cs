using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterPlayLoader : AbilityLoader
{
    public OnEnterPlayLoader(Card owner) : base(owner) { }

    public override void TriggerAbility()
    {
        if (observer.owner.inPlay)
        {
            actions[0].Execute();
        }
    }
}
