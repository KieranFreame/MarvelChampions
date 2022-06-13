using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public CardData data;
    public string actionName { get; set; }
    public int value { get; set; }
    public string trigger {get; set;}

    public Action(ActionData data)
    {
        actionName = data.actionName;
        value = data.value;
        this.trigger = data.trigger;
    }

    public Action(string actionName, int value)
    {
        this.actionName = actionName;
        this.value = value;
    }

    public Action(string actionName)
    {
        this.actionName = actionName;
    }

    public abstract void Execute();
}
