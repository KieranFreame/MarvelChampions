using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExhaust
{
    bool Exhausted { get; set; }
    public void Ready();
    public void Exhaust();
}
