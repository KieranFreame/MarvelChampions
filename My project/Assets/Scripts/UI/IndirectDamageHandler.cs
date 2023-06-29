using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IndirectDamageHandler : MonoBehaviour
{
    private static IndirectDamageHandler inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);

        UI = FindObjectOfType<IndirectDamageUI>(true);
    }

    private int _damageToApply = 0;
    private List<ICharacter> _candidates = new();
    private readonly List<DamageAction> _actions = new();

    private IndirectDamageUI UI;

    public static void HandleIndirectDamage(List<ICharacter> candidates,int damage)
    {
        inst._candidates = candidates;
        inst._damageToApply = damage;
        inst._actions.Clear();

        inst.IndirectDamage();
    }

    private async void IndirectDamage()
    {
        ICharacter h;

        while (_damageToApply > 0)
        {
            h = await TargetSystem.instance.SelectTarget(_candidates);
            UI.gameObject.SetActive(true);

            int damageApplied = await UI.SetIndirectDamage(h);
            _actions.Add(new(h, damageApplied));
            _damageToApply -= damageApplied;
        }

        foreach (DamageAction d in _actions)
        {
           await DamageSystem.instance.ApplyDamage(d);
        }
    }
}
