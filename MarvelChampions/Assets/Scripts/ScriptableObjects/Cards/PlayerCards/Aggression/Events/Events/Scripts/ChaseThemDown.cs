using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase Them Down", menuName = "MarvelChampions/Card Effects/Aggression/Chase Them Down")]
public class ChaseThemDown : PlayerCardEffect, IOptional
{
    public override void OnDrawn()
    {
        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Owner == _owner as ICharacter)
            if (action.Target.CharStats.Health.CurrentHealth <= 0 && ScenarioManager.inst.ThreatPresent())
                EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override async Task Resolve()
    {
        await PlayCardSystem.Instance.InitiatePlayCard(new(_card));
    }

    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateThwart(new(2, Owner));
    }

    public override void OnDiscard()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
    }
}
