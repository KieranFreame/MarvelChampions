using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlterEgo : MonoBehaviour, IIdentity
{
    #region IIdentity
    public string idName { get; set; }
    public int baseHandSize { get; set; }
    public int handSize { get; set; }
    public int baseHP { get; set; }
    public int hp { get; set; }

    public virtual void SwitchIdentity() { }
    #endregion

    public GameEvent onFlip;

    public int baseREC { get; set; }
    public int REC { get; set; }

    public virtual void Setup() { return; }
    public virtual void Effect() { return; }
}
