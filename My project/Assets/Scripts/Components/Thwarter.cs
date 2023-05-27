using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Thwarter : IConfusable, IStat
{
    #region Properties
    private int _currThwart;
    public int CurrentThwart 
    {
        get => _currThwart;
        set
        {
            _currThwart = value;
            ThwartChanged?.Invoke();
        } 
    }

    public int BaseThwart { get; private set; }

    private bool _confused = false;
    public bool Confused
    {
        get { return _confused; }
        set
        {
            _confused = value;
            OnToggleConfuse?.Invoke(_confused);
        }
    }
    #endregion

    public dynamic Owner { get; private set; }

    #region Events
    public event UnityAction<bool> OnToggleConfuse;
    public event UnityAction ThwartChanged;
    #endregion

    #region Constructors
    public Thwarter(AllyCard owner, AllyCardData data)
    {
        Owner = owner;
        CurrentThwart = BaseThwart = data.BaseTHW;
    }
    public Thwarter(Identity owner, HeroData data)
    {
        Owner = owner;
        CurrentThwart = BaseThwart = data.baseTHW;
    }
    #endregion

    public ThwartAction Thwart()
    {
        if ((Owner as IExhaust).Exhausted)
            return null;

        (Owner as IExhaust).Exhaust();

        if (Confused)
        {
            Confused = false;
            return null;
        }

        return new ThwartAction(_thwart:CurrentThwart);
    }
}
