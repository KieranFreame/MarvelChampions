using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IIdentity, IStatus, ICombatant, IDestructable
{
    #region IIdentity
    public string idName { get; set; }
    public int baseHandSize { get; set; }
    public int handSize { get; set; }
    public virtual void SwitchIdentity() { }
    #endregion

    #region IStatus
    public bool stunned { get; set; }
    public bool confused { get; set; }
    public bool tough { get; set; }
    #endregion

    #region ICombatant
    public int baseAttack { get; set; }
    public int attack { get; set; }
    public void AttemptAttack()
    {
        // Step 1: Exhaust
        if (ready)
            Exhaust();
        else
            return;

        //Step 2: Check for Stun
        if (stunned)
        {
            stunned = false;
            return;
        }

        StartCoroutine(Attack());
    }
    public virtual IEnumerator Attack()
    {
        //Step 3: Get Target
        StartCoroutine(TargetEnemySystem.instance.GetTarget());

        var target = TargetEnemySystem.instance.target.GetComponent<IDestructable>();

        //Step 4: Send Attack Event
        if (target != null)
            AttackSystem.instance.Attack(new AttackAction(this, target, new List<string>()));

        yield return null;
    }
    #endregion

    #region IDestructable
    public Health hitpoints { get; set; }
    public virtual void WhenDefeated(){}
    #endregion

    #region Hero
    bool ready = true;
    public int baseDefence { get; set; }
    public int defence { get; set; }

    protected List<string> keywords = new List<string>();
    public void Exhaust()
    {
        if (ready)
        {
            ready = false;
            GetComponent<Animator>().Play("Exhaust");
        }
        else
        {
            ready = true;
            GetComponent<Animator>().Play("Ready");
        }
    }
    protected virtual void Awake()
    {
        hitpoints = GetComponent<Health>();
    }
    #endregion
}
