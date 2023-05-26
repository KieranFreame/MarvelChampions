using System.Collections;
using System.Collections.Generic;
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
    private List<Health> _candidates = new();
    private List<DamageAction> _actions = new();

    private IndirectDamageUI UI;

    public static void HandleIndirectDamage(List<Health> candidates, int damage)
    {
        inst._candidates = candidates;
        inst._damageToApply = damage;
        inst._actions.Clear();

        inst.StartCoroutine(inst.IndirectDamage());
    }

    private IEnumerator IndirectDamage()
    {
        Health h = null;

        while (_damageToApply > 0)
        {
            yield return StartCoroutine(TargetSystem.instance.SelectTarget(_candidates, health =>
            {
                h = health;
                UI.gameObject.SetActive(true);
            }));

            yield return StartCoroutine(UI.SetIndirectDamage(h, damageApplied =>
            {
                _actions.Add(new(h, damageApplied));
                _damageToApply -= damageApplied;
            }));

            yield return null;
        }

        foreach (DamageAction d in _actions)
        {
            yield return StartCoroutine(DamageSystem.ApplyDamage(d));
        }
    }
}
