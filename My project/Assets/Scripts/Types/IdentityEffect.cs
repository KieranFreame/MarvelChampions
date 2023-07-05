using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdentityEffect : ScriptableObject
{
    protected Player owner;
    protected bool hasActivated = false;
    public abstract void LoadEffect(Player _owner);
    public virtual void OnFlipUp() { }
    public virtual void OnFlipDown() { }
    public virtual bool CanActivate() { return false; }
    public virtual void Activate() { }

    protected virtual void Reset() => hasActivated = false;
}
