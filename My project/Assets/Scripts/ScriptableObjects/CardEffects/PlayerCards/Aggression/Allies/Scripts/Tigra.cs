using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tigra", menuName = "MarvelChampions/Card Effects/Aggression/Tigra")]
public class Tigra : PlayerCardEffect
{
    MinionCard _target;

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        Card = card;

        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
        await Task.Yield();
    }

    private void AttackInitiated() => TargetSystem.TargetAcquired += CheckTarget;

    private void CheckTarget(dynamic target)
    {
        TargetSystem.TargetAcquired -= CheckTarget;

        if (target is MinionCard)
        {
            _target = target;
            AttackSystem.OnAttackComplete += AttackComplete;
        }   
    }

    private void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;

        if (_target.CharStats.Health.CurrentHealth == 0) //defeated
        {
            (Card as AllyCard).CharStats.Health.RecoverHealth(1);
        }
    }

    public override void OnExitPlay()
    {
        (Card as AllyCard).CharStats.AttackInitiated -= AttackInitiated;
    }
}
