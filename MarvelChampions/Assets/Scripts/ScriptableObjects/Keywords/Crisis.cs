using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crisis
{
    SchemeCard owner;

    public Crisis(SchemeCard card)
    {
        owner = card;
        ThwartSystem.Instance.Crisis.Add(owner);
        owner.Threat.WhenDefeated += WhenDefeated;
    }

    public void WhenDefeated()
    {
        ThwartSystem.Instance.Crisis.Remove(owner);
        owner.Threat.WhenDefeated -= WhenDefeated;
    }
}
