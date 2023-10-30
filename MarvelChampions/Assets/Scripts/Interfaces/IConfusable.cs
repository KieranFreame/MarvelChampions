using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IConfusable
{
    bool Confused { get; set; }

    event UnityAction<bool> OnToggleConfuse;
}
