using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public string EffectID;
    protected Card _card;

    public virtual bool CanActivate() { return false; }
    public virtual void OnEnterPlay(Player owner, Card card) { }
    public virtual void OnEnterPlay(Villain owner, Card card) { }
    public virtual void Activate() { }
    public virtual void WhenDefeated() { }
    public virtual void OnExitPlay() { }
}
