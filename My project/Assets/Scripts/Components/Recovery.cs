using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Recovery
{
    private int _recovery;
    public int REC
    {
        get => _recovery;
        set
        {
            _recovery = value;
            RecoveryChanged?.Invoke();
        }
    }
    public int BaseREC { get; private set; }

    //public event UnityAction OnRecover;
    public event UnityAction RecoveryChanged;

    public Identity Owner { get; private set; }

    public Recovery(Identity owner, AlterEgoData data)
    {
        Owner = owner;
        REC = BaseREC = data.baseREC;
    }

    public void Recover()
    {
        Owner.CharStats.Health.RecoverHealth(REC);
        Owner.Exhaust();
        //OnRecover?.Invoke();
    }
    
}
