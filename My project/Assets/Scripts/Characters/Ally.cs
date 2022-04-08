using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Card, ICombatant, IThwarter, IDestructable, IStatus
{
    void Start()
    {
        hitpoints = GetComponent<Health>();
        hitpoints.currHealth = hitpoints.BaseHP = (_cd as AllyData).baseHp;
        attack = baseAttack = (_cd as AllyData).baseAttack;
        thwart = baseThwart = (_cd as AllyData).baseThwart;
        (_cd as AllyData).ready = true;
    }

    public void Exhaust()
    {
        if (!(_cd as AllyData).ready)
            return;
        
        (_cd as AllyData).ready = false;
        GetComponent<Animator>().Play("Exhaust");
        
    }

    public void Ready()
    {
        if ((_cd as AllyData).ready)
            return;

        (_cd as AllyData).ready = true;
        GetComponent<Animator>().Play("Ready");
    }

    #region IDestructable
    public Health hitpoints { get; set; }
    public virtual void WhenDefeated() { }
    #endregion

    #region ICombatant
    public int baseAttack { get; set; }
    public int attack { get; set; }

    public virtual void AttemptAttack()
    {
        // Step 1: Exhaust
        if ((_cd as AllyData).ready)
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
    public IEnumerator Attack()
    {
        //Step 3: Get Target
        yield return StartCoroutine(TargetEnemySystem.instance.GetTarget());

        var target = TargetEnemySystem.instance.target.GetComponent<IDestructable>();

        //Step 4: Send Attack Event
        if (target != null)
            AttackSystem.instance.Attack(new AttackAction(this, target, new List<string>()));

        yield return null;
    }
    #endregion

    #region IThwarter
    public int baseThwart { get; set; }
    public int thwart { get; set; }
    public virtual void AttemptThwart()
    {
        // Step 1: Exhaust
        if ((_cd as AllyData).ready)
            Exhaust();
        else
            return;

        //Step 2: Check for Confuse
        if (confused)
        {
            confused = false;
            return;
        }

        StartCoroutine(Thwart());
    }
    public IEnumerator Thwart()
    {
        //Step 3: Get Target
        yield return StartCoroutine(TargetSchemeSystem.instance.GetTarget());

        var target = TargetSchemeSystem.instance.target.GetComponent<IScheme>();

        //Step 4: Send Thwart Event
        if (target != null)
            ThwartSystem.instance.Thwart(new ThwartAction(this, target, new List<string>()));

        yield return null;
    }
    #endregion

    #region IStatus
    public bool stunned { get; set; }
    public bool confused { get; set; }
    public bool tough { get; set; }
    #endregion
}
