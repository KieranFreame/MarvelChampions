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
        _health = _owner.CharStats.Health;

        _health.Defeated += WhenDefeated;

        AttackSystem.OnAttackComplete += Effect;
    }

    /// <summary>
    /// Use for Allies & Minions
    /// </summary>
    public Retaliate(ICharacter owner, int damage)
    {
        _owner = owner;
        _damage = damage;
        _health = _owner.CharStats.Health;

        _health.Defeated += WhenDefeated;
        AttackSystem.OnAttackComplete += Effect;
    }

    private async void Effect(Action action)
    {
        if (AttackSystem.instance.Target != _owner.CharStats.Health)
            return;

        if (action.Owner != null)
            await DamageSystem.instance.ApplyDamage(new(action.Owner, _damage));
    }

    private void WhenDefeated()
    {
        _health.Defeated -= WhenDefeated;
        AttackSystem.OnAttackComplete -= Effect;
    }
}
