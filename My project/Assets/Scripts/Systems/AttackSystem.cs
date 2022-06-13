using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static AttackSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    #region Fields
    public int excess = 0;
    #endregion

    public void InitiateAttack(AttackAction attack) => StartCoroutine(AttackSystem.instance.Attack(attack));

    private IEnumerator Attack(AttackAction attack)
    {
        yield return StartCoroutine(TargetSystem.instance.GetTarget("Enemy", "Health"));
        var target = TargetSystem.instance.target;

        if (attack.owner.keywords.Contains("Piercing"))
            target.tough = false;

        if (attack.owner.keywords.Contains("Overkill") /*&& attack.target.GetComponent<Villain>() == null*/)
            excess = attack.owner._attack - target.currHealth;

        target.TakeDamage(attack.owner._attack);

        if (excess > 0)
            Overkill();

        yield return null;
    }

    private void Overkill()
    {
        if (excess > 0 /*&& GameManager.instance.scenario.villain != null*/)
        {
            //(GameManager.instance.scenario.villain as MonoBehaviour).gameObject.SendMessage("TakeDamage", excess, SendMessageOptions.DontRequireReceiver);
        }
    }
}
