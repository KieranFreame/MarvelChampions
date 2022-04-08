using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Encounter, IDestructable, ICombatant, IStatus 
{
    #region IStatus
    public bool stunned { get; set; }
    public bool confused { get; set; }
    public bool tough { get; set; }
    #endregion

    #region ICombatant
    public int baseAttack { get; set; }
    public int attack { get; set; }
    public virtual void AttemptAttack() { }
    public virtual void Attack(Player player) { }
    #endregion

    public int baseScheme { get; protected set; }
    public int scheme { get; set; }
    public virtual void Scheme() { }

    #region IDestructable
    public Health hitpoints { get; set; }
    public virtual void WhenDefeated() { GameManager.instance.scenario.Discard(this); }
    #endregion

    public virtual void Ability() {}
    
}
