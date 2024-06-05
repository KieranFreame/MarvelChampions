using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Retaliate
{
    private ICharacter _owner;
    private Health _health;
    [SerializeField] private int _damage;

    public Retaliate(ICharacter owner, int damage)
    {
        _owner = owner;
        _damage = damage;
        _health = _owner.CharStats.Health;

        _health.Defeated.Add(WhenDefeated);
        _health.OnTakeDamage += Effect;
    }

    private async void Effect(DamageAction action)
    {
        if (action.IsAttack)
            await DamageSystem.Instance.ApplyDamage(new(action.Owner, _damage));
    }

    private void WhenDefeated()
    {
        _health.Defeated.Remove(WhenDefeated);
        _health.OnTakeDamage -= Effect;
    }

    public void WhenRemoved()
    {
        _health.Defeated.Remove(WhenDefeated);
        _health.OnTakeDamage -= Effect;
    }

    public void OnFlip(bool subscribe)
    {
        if (subscribe)
        {
            _health.Defeated.Add(WhenDefeated);
            _health.OnTakeDamage += Effect;
        }
        else
        {
            _health.Defeated.Remove(WhenDefeated);
            _health.OnTakeDamage -= Effect;
        }
    }
}
