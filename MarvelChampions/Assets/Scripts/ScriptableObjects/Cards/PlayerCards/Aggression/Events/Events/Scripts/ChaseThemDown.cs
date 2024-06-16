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
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;
    }

    private void IsTriggerMet(Action action)
    {
        if (action is not AttackAction || action.Owner.Name == _owner.Name) return;

        var attack = (AttackAction)action;

        if (attack.Target.CharStats.Health.CurrentHealth <= 0 && ScenarioManager.inst.ThreatPresent())
            EffectManager.Inst.Responding.Add(this);
    }

    public override async Task Resolve()
    {
        await _owner.CharStats.InitiateThwart(new(2, Owner));
    }

    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
        return Task.CompletedTask;
    }

    public override void OnDiscard()
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
    }
}
