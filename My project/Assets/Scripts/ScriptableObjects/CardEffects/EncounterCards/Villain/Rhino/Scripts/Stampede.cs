using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stampede", menuName = "MarvelChampions/Card Effects/Rhino/Stampede")]
public class Stampede : EncounterCardEffect
{
    ICharacter _target;
    int charHP;

    /// <summary>
    /// When Revealed (Alter-Ego): This card gains surge.
    /// When Revealed(Hero): Rhino attacks you.If a character is damaged by this attack, that character is stunned.
    /// </summary>
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        _target = player;
        var identity = player.Identity.ActiveIdentity;

        if (identity is AlterEgo)
        {
            _owner.Surge(player);
        }
        else //identity is Hero;
        {
            TargetSystem.TargetAcquired += SetDefender;
            await _owner.CharStats.InitiateAttack();
        } 
    }

    private void SetDefender(dynamic target)
    {
        TargetSystem.TargetAcquired -= SetDefender;
        AttackSystem.OnAttackComplete += AttackComplete;

        if (target != null)
        {
            _target = target as ICharacter;
        }

        charHP = _target.CharStats.Health.CurrentHealth;
    }

    private void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;

        if (_target.CharStats.Health.CurrentHealth < charHP && _target.CharStats.Health.CurrentHealth != 0) //target has taken damage, but is not defeated
        {
            _target.CharStats.Attacker.Stunned = true;
        }
    }
}
