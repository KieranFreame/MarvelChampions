using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DamageSystem
{
    private static DamageSystem instance;

    public static DamageSystem Instance
    {
        get
        {
            if (instance == null)
                instance = new DamageSystem();

            return instance;
        }
    }

    DamageSystem()
    {
        PauseMenu.OnRestartGame += Restart;
    }

    private void Restart()
    {
        PauseMenu.OnRestartGame -= Restart;
        instance = null;
    }

    #region Delegate
    public delegate Task<DamageAction> ModifyDamage(DamageAction action);
    public List<ModifyDamage> Modifiers { get; private set; } = new();
    #endregion

    public async Task ApplyDamage(DamageAction action)
    {
        if (action.DamageTargets.Count > 1 && !action.TargetAll)
        {
            var target = await TargetSystem.instance.SelectTarget(action.DamageTargets);
            action.DamageTargets.RemoveAll(x => x != target);
        }

        for (int i = Modifiers.Count - 1; i >= 0; i--)
        {
            action = await Modifiers[i](action);
        }

        foreach (ICharacter target in action.DamageTargets)
        {
            target.CharStats.Health.TakeDamage(action);
        }
    }
}

