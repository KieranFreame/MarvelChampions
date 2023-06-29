using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void Enter()
    {
        UIManager.MakingSelection = false;
    }

    public override void Exit()
    {
        UIManager.MakingSelection = true;
    }
}
