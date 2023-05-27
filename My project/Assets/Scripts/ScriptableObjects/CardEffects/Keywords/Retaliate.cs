using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliate
{
    private dynamic _owner;
    private Health _health;
    [SerializeField] private int _damage;

    /// <summary>
    /// Use for Identities, Villains or Permanents (Zola, Black Panther, Cap's Shield)
    /// </summary>
    public Retaliate(dynamic owner, int damage)
    {
        _owner = owner;
        _damage = damage;
        _health = (_owner is Villain) ? (_owner as Villain).GetComponent<Health>() : (_owner as Player).GetComponent<Health>();

        _health.Defeated += WhenDefeated;

        AttackSystem.OnAttackComplete += Effect;
    }

    /// <summary>
    /// Use for Allies & Minions
    /// </summary>
    public Retaliate(Card owner, int damage) //Allies & Minions
    {
        _owner = owner;
        _damage = damage;
        _health = (_owner as Card).GetComponent<Health>();

        _health.Defeated += WhenDefeated;
        AttackSystem.OnAttackComplete += Effect;
    }

    private void Effect(Action action)
    {
        if (AttackSystem.instance.Target != _owner.GetComponent<Health>())
            return;

        action.Owner.TryGetComponent(out Health target);

        if (target != null)
            _owner.StartCoroutine(DamageSystem.instance.ApplyDamage(new DamageAction(target, _damage)));
    }

    private void WhenDefeated()
    {
        _health.Defeated -= WhenDefeated;
        AttackSystem.OnAttackComplete -= Effect;
    }
}
