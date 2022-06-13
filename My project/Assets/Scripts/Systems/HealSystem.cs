using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSystem : MonoBehaviour
{
    #region SingletonPattern
    public static HealSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public void InitiateHeal(HealAction action) => StartCoroutine(Heal(action));

    private IEnumerator Heal(HealAction action)
    {
        if (action.targetPlayer)
        {
            var playerHealth = GameObject.Find("PlayerUI").GetComponent<Health>();

            playerHealth.RecoverHealth(action.heal);
            yield return null;
        }

        yield return StartCoroutine(TargetSystem.instance.GetTarget("Ally", "Health"));
        var health = TargetSystem.instance.target;

        health.RecoverHealth(action.heal);
        yield return null;
    }
}
