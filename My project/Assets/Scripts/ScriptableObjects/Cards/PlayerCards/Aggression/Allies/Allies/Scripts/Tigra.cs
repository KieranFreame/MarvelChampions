using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tigra", menuName = "MarvelChampions/Card Effects/Aggression/Tigra")]
public class Tigra : PlayerCardEffect
{
    MinionCard _target;

    public override Task OnEnterPlay()
    {
        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        if (_target != null)
            _target.CharStats.Health.Defeated.Remove(Defeated);

        AttackSystem.TargetAcquired += CheckTarget;
    }

    private void CheckTarget(ICharacter target)
    {
        AttackSystem.TargetAcquired -= CheckTarget;

        if (target is MinionCard)
        {
            _target = target as MinionCard;
            _target.CharStats.Health.Defeated.Add(Defeated);
        }   
    }

    private Task Defeated()
    {
        _target.CharStats.Health.Defeated.Remove(Defeated);
        (Card as AllyCard).CharStats.Health.RecoverHealth(1);
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        (Card as AllyCard).CharStats.AttackInitiated += AttackInitiated;
    }
}
