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

        GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
        _health.OnTakeDamage += Effect;
    }

    private async void Effect(DamageAction action)
    {
        if (action.IsAttack)
            await DamageSystem.Instance.ApplyDamage(new(action.Owner, _damage));
    }

    private void WhenDefeated(ICharacter defeated)
    {
        if (defeated != _owner) return;

        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
        _health.OnTakeDamage -= Effect;
    }

    public void WhenRemoved()
    {
        GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
        _health.OnTakeDamage -= Effect;
    }

    public void OnFlip(bool subscribe)
    {
        if (subscribe)
        {
            GameStateManager.Instance.OnCharacterDefeated += WhenDefeated;
            _health.OnTakeDamage += Effect;
        }
        else
        {
            GameStateManager.Instance.OnCharacterDefeated -= WhenDefeated;
            _health.OnTakeDamage -= Effect;
        }
    }
}
