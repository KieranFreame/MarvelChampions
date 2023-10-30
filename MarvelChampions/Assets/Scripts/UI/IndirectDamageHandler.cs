using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IndirectDamageHandler : MonoBehaviour
{
    public static IndirectDamageHandler inst;

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

    public async Task HandleIndirectDamage(List<ICharacter> candidates,int damage)
    {
        _candidates = candidates;
        _damageToApply = damage;
        _actions.Clear();

        Debug.Log("Dealing Indirect Damage: " + _damageToApply);

        await inst.IndirectDamage();
    }

    private async Task IndirectDamage()
    {
        ICharacter h;

        while (_damageToApply > 0)
        {
            h = await TargetSystem.instance.SelectTarget(_candidates);
            UI.gameObject.SetActive(true);

            int damageApplied = await UI.SetIndirectDamage(h);
            _actions.Add(new(h, damageApplied));
            _damageToApply -= damageApplied;
            Debug.Log("Damage Remaining: " + _damageToApply);
        }

        foreach (DamageAction d in _actions)
        {
           await DamageSystem.Instance.ApplyDamage(d);
        }
    }
}
