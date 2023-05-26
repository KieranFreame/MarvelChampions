using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdentityEffect : ScriptableObject
{
    protected Player owner;
    public bool hasActivated = false;
    public abstract void LoadEffect(Player _owner);
    public virtual void OnFlip() { }
    public virtual bool CanActivate() { return true; }
    public virtual void Activate() { }

    protected virtual void Reset() => hasActivated = false;
}
