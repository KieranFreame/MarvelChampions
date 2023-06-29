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

    #region Delegates
    public List<IModifyDamage> Modifiers { get; private set; } = new List<IModifyDamage>();
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
            DamageAction a = new(target, action.Value);

            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                if (Modifiers[i] == null)
                {
                    Modifiers.RemoveAt(i);
                    continue;
                }

                a = await Modifiers[i].OnTakeDamage(a, target);
                if (action.Value < 0) a.Value = 0;
            }

           target.CharStats.Health.TakeDamage(a.Value);
        }

        OnDamageApplied?.Invoke();
    }
}

