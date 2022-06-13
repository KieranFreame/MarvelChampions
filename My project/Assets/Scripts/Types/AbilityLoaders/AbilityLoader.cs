using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityLoader
{
    public List<Action> actions;
    public Observer observer { get; set; }

    public AbilityLoader(Card owner)
    {
        observer = new Observer(owner);
        actions = new List<Action>();
    }

    public virtual void TriggerAbility()
    {
        actions[0].Execute();
    }

    public void AddAction(Action action)
    {
        actions.Add(action);

        if (actions.Count == 1)
            observer.trigger = actions[0].trigger;
    }
}
