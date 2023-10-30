using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionData
{
    public ActionType actionName;
    public List<TargetType> targets;
    public int value;
    public string requirement;
}
