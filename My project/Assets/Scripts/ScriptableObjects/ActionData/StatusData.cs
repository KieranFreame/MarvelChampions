using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Data", menuName= "ActionData/Status")]
public class StatusData : ActionData
{
    public Status status;
    public bool targetSelf;
}
