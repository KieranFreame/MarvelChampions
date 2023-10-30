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
            ScenarioManager.inst.Surge(player);
        }
        else //identity is Hero;
        {
            DefendSystem.Instance.OnTargetSelected += SetDefender;
            await _owner.CharStats.InitiateAttack();
        } 
    }

    private void SetDefender(ICharacter target)
    {
        DefendSystem.Instance.OnTargetSelected -= SetDefender;

        _target = target;
        _target.CharStats.Health.OnTakeDamage += AttackComplete;
    }

    private void AttackComplete(DamageAction action)
    {
        _target.CharStats.Health.OnTakeDamage -= AttackComplete;

        if (action.Value > 0)
        {
            _target.CharStats.Attacker.Stunned = true;
        }
    }
}
