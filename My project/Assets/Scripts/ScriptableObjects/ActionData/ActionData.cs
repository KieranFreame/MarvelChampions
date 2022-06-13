using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action Data", menuName = "ActionData/Base")]
public class ActionData : ScriptableObject
{
    public CardData data;
    public string actionName;
    public int value;
    public string trigger;
}
