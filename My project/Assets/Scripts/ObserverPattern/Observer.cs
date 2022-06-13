using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer //: MonoBehaviour
{
    public Card owner { get; set; }
    public string trigger { get; set; }

    public Observer(Card owner)
    {
        this.owner = owner;
    }

    public virtual void Notify()
    {
        owner.abilityLoader.TriggerAbility();
    }
}
