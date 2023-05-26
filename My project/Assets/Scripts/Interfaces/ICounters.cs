using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICounters
{
    public int Counters { get; set; }

    public event UnityAction<int, GameObject> CounterIncrement;
    public event UnityAction<int, GameObject> CounterDecrement;
}
