using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard
{
    private dynamic _owner;
    private Card _card;
    public Guard(dynamic owner, Card card)
    {
        _owner = owner;
        _card = card;

        TargetSystem.CheckGuard += Effect;
        _card.GetComponent<Health>().Defeated += WhenDefeated;
    }

    private void Effect()
    {
        if (TargetSystem.instance.candidates.Contains(_owner.GetComponent<Health>()))
            TargetSystem.instance.candidates.Remove(_owner.GetComponent<Health>());
    }

    private void WhenDefeated()
    {
        TargetSystem.CheckGuard -= Effect;
        _card.GetComponent<Health>().Defeated -= WhenDefeated;
    }
}
