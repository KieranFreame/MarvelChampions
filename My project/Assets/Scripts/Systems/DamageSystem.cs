using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static DamageSystem instance;

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

    public void InitiateDamage(DamageAction damage) => StartCoroutine(Damage(damage));

    private IEnumerator Damage(DamageAction damage)
    {
        yield return StartCoroutine(TargetSystem.instance.GetTarget("Enemy", "Health"));
        var target = TargetSystem.instance.target;

        if (target is Health)
            target.TakeDamage(damage.value);
        else
        {
            target.GetComponent<Health>().TakeDamage(damage.value);
        }

        yield return null;
    }
}
