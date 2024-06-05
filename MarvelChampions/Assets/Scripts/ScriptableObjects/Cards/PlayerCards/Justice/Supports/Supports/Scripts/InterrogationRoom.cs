using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Interrogation Room", menuName = "MarvelChampions/Card Effects/Justice/Interrogation Room")]
public class InterrogationRoom : PlayerCardEffect, IOptional
{
    public override Task OnEnterPlay()
    {
        AttackSystem.TargetAcquired += CheckTarget;
        return Task.CompletedTask;
    }

    public override async Task Resolve()
    {
        _card.Exhaust();
        await ThwartSystem.Instance.InitiateThwart(new(1, null));
    }

    private void CheckTarget(ICharacter target)
    {
        if (target is MinionCard)
            AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Target.CharStats.Health.CurrentHealth <= 0 && ScenarioManager.inst.ThreatPresent() && !_card.Exhausted)
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);

        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
    }

    public override void OnExitPlay()
    {
        AttackSystem.TargetAcquired -= CheckTarget;
    }
}
