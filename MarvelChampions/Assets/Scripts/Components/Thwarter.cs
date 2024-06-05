using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Thwarter : IConfusable
{
    #region Properties
    private int _currThwart;
    public int CurrentThwart 
    {
        get => _currThwart;
        set
        {
            _currThwart = value;

            if (_currThwart < BaseThwart)
                _currThwart = BaseThwart;

            ThwartChanged?.Invoke();
        } 
    }

    public int BaseThwart { get; set; }

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

    public ICharacter Owner { get; private set; }

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
    public Thwarter(Player owner, HeroData data)
    {
        Owner = owner;
        CurrentThwart = BaseThwart = data.baseTHW;
    }
    #endregion

    public ThwartAction Thwart(ThwartAction action = null)
    {
        if (Owner is IExhaust && action == null)
        {
            if ((Owner as IExhaust).Exhausted)
                return null;

            (Owner as IExhaust).Exhaust();
        }

        if (Confused)
        {
            Confused = false;
            return null;
        }

        return action ?? new ThwartAction(_thwart:CurrentThwart, owner: Owner);
    }
}
