using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : IState
{
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
