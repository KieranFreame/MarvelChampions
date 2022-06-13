using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeValueAction Data", menuName = "ActionData/ChangeValueAction")]
public class ChangeValueData : ActionData
{
    public Value valueToChange;
    public Operation operation;
}
