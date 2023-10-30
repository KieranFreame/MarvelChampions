using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Green Goblin", menuName = "MarvelChampions/Villain Effects/Mutagen Formula")]
public class GreenGoblin : VillainEffect
{
    public override void LoadEffect(Villain owner)
    {
        _owner = owner;
        _owner.CharStats.AttackInitiated += AttackInitiated;
    }

    private void AttackInitiated()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
    }

    private void DefenderSelected(ICharacter target)
    {
        if (target is not Player) return;

        target.CharStats.Health.OnTakeDamage += OnDamageDealt;
    }

    private void OnDamageDealt(DamageAction action)
    {
        if (action.Value > 0)
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat((_owner.Stages.Stage == 3) ? 2 : 1);
        }
    }

    public override Task StageTwoEffect()
    {
        foreach (var p in TurnManager.Players)
        {
            for (int i = 0; i < 2; i++)
            {
                ScenarioManager.inst.Surge(p);
            }
        }

        return Task.CompletedTask;
    }

    public override Task StageThreeEffect()
    {
        foreach (var p in TurnManager.Players)
        {
            for (int i = 0; i < 3; i++)
            {
                ScenarioManager.inst.Surge(p);
            }
        }
;
        return Task.CompletedTask;
    }
}
