using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Doctor Strange", menuName = "MarvelChampions/Identity Effects/Doctor Strange/Hero")]
public class DoctorStrange : IdentityEffect
{
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
    }

    public override bool CanActivate()
    {
        if (owner.Exhausted)
            return false;

        if (owner.ResourcesAvailable() < InvocationDeck.Instance.Invocations[0].cardCost)
            return false;

        return InvocationDeck.Instance.CanActivate();
    }

    public override async void Activate()
    {
        owner.Exhaust();
        await PayCostSystem.instance.GetResources(amount: InvocationDeck.Instance.Invocations[0].cardCost);
        await InvocationDeck.Instance.Activate();
        InvocationDeck.Instance.Discard();
    }
}
