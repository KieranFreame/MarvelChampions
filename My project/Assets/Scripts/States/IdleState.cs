using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter()
    {
        UIManager.InStateMachine = false;
    }

    public override void Exit()
    {
        UIManager.InStateMachine = true;
    }
}
