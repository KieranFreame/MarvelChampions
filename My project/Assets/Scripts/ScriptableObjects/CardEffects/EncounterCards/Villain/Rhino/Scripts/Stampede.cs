using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stampede", menuName = "MarvelChampions/Card Effects/Rhino/Stampede")]
public class Stampede : EncounterCardEffect
{
    int charHP;

    /// <summary>
    /// When Revealed (Alter-Ego): This card gains surge.
    /// When Revealed(Hero): Rhino attacks you.If a character is damaged by this attack, that character is stunned.
    /// </summary>
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        var identity = FindObjectOfType<Player>().Identity.ActiveIdentity;

        if (identity is AlterEgo)
        {
            _owner.Surge(FindObjectOfType<Player>());
        }
        else //identity is Hero;
        {
            _owner.StartCoroutine(_owner.CharStats.InitiateAttack());

            DefendSystem.instance.OnDefenderSelected += SetDefender;
            AttackSystem.OnActivationComplete += AttackComplete;
        } 
    }

    private void SetDefender()
    {
        charHP = (DefendSystem.instance.Target != null) ? DefendSystem.instance.Target.CharStats.Health.CurrentHealth : TurnManager.instance.CurrPlayer.CharStats.Health.CurrentHealth;
    }

    private void AttackComplete()
    {
        var target = AttackSystem.instance.Target;

        if (target != null) //defending ally was defeated
        {
            if (target.CharStats.Health.CurrentHealth < charHP) //target has taken damage
            {
                target.CharStats.Attacker.Stunned = true;
            }
        }

        DefendSystem.instance.OnDefenderSelected -= SetDefender;
        AttackSystem.OnActivationComplete -= AttackComplete;
    }
}
