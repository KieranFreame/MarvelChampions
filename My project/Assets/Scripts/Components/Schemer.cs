using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Schemer : IConfusable, IStat
{
    private int _scheme;
    public int BaseScheme { get; private set; }
    private bool _confused;
    public List<string> keywords = new(); //temp?

    public dynamic Owner { get; private set; }

    public event UnityAction<bool> OnToggleConfuse;
    public event UnityAction SchemeChanged;

    public Schemer(Villain owner)
    {
        Owner = owner;
        CurrentScheme = BaseScheme = owner.BaseScheme;
    }

    public Schemer(MinionCard owner, MinionCardData data)
    {
        Owner = owner;
        CurrentScheme = BaseScheme = data.baseScheme;
    }

    public SchemeAction Scheme()
    {
        if (Confused)
        {
            Confused = false;
            return null;
        }

        var scheme = new SchemeAction(_scheme, Owner);
        return scheme;
    }

    public bool Confused
    {
        get => _confused;
        set 
        { 
            _confused = value;
            OnToggleConfuse?.Invoke(_confused);
        }
    }

    public int CurrentScheme
    {
        get { return _scheme; }
        set
        {
            _scheme = value;
            SchemeChanged?.Invoke();
        }
    }
}
