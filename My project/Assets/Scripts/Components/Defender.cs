using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Defender : IStat
{
    #region Properties
    private int _defence;
    public int CurrentDefence
    {
        get => _defence;
        set
        {
            _defence = value;
            DefenceChanged?.Invoke();
        }
    }
    public int BaseDEF { get; private set; }
    #endregion

    private readonly Identity _owner;

    #region Events
    public event UnityAction DefenceChanged;
    #endregion

    public Defender(Identity owner, HeroData data)
    {
        _owner = owner;
        CurrentDefence = BaseDEF = data.baseDEF;
    }

    public int Defend()
    {
        _owner.Exhaust();
        return _defence;
    }
}
