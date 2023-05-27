using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Recovery : IStat
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

    private Health _health;

    //public event UnityAction OnRecover;
    public event UnityAction RecoveryChanged;

    public Identity Owner { get; private set; }

    public Recovery(Identity owner, Health health, AlterEgoData data)
    {
        Owner = owner;
        REC = BaseREC = data.baseREC;
        _health = health;
    }

    public void Recover()
    {
        _health.RecoverHealth(REC);
        Owner.Exhaust();
        //OnRecover?.Invoke();
    }
    
}
