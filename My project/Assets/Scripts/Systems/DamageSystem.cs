using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    #region Fields
    public DamageAction DamageAction { get; private set; }
    #endregion

    #region Events
    public static event UnityAction OnDamageApplied;
    #endregion

    #region Delegates
    public List<IModifyDamage> Modifiers { get; private set; } = new List<IModifyDamage>();
    #endregion

    public IEnumerator ApplyDamage(DamageAction action)
    {
        DamageAction = action;

        if (DamageAction.DamageTargets.Count == 0)
        {
            yield return StartCoroutine(TargetSystem.instance.SelectTarget(DamageAction.DamageTargets, character =>
            {
                DamageAction.DamageTargets.Add(character.CharStats.Health);
            }));
        }

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            if (Modifiers[i] == null)
            {
                Modifiers.RemoveAt(i);
                continue;
            }

            yield return (StartCoroutine(Modifiers[i].OnTakeDamage(DamageAction, action =>
            {
                if (action.Value < 0) action.Value = 0;
                DamageAction = action;
            })));
        }

        foreach (Health h in DamageAction.DamageTargets)
            h.TakeDamage(DamageAction.Value);

        OnDamageApplied?.Invoke();
    }
}

