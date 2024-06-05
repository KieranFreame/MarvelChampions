using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tigra", menuName = "MarvelChampions/Card Effects/Aggression/Tigra")]
public class Tigra : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        AttackSystem.TargetAcquired += CheckTarget;
        return Task.CompletedTask;
    }

    private void CheckTarget(ICharacter target)
    {
        if (target is MinionCard)
        {
            AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
        }   
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Target == null && action.Owner == Card as ICharacter)
        {
            (Card as AllyCard).CharStats.Health.CurrentHealth += 1;
        }
    }

    public override void OnExitPlay()
    {
        AttackSystem.TargetAcquired -= CheckTarget;
    }
}
