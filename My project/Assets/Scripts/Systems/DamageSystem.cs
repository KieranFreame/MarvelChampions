using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class DamageSystem : MonoBehaviour
{
    public static DamageSystem instance;

    private void Awake()
    {
        //Singleton Pattern
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Events
    public static event UnityAction OnDamageApplied;
    #endregion

    public async Task ApplyDamage(DamageAction action)
    {
        if (action.DamageTargets.Count > 1 && !action.TargetAll)
        {
            var target = await TargetSystem.instance.SelectTarget(action.DamageTargets);
            action.DamageTargets.RemoveAll(x => x != target);
        }

        foreach (ICharacter target in action.DamageTargets)
        {
           target.CharStats.Health.TakeDamage(action.Value);
        }

        OnDamageApplied?.Invoke();
    }
}

