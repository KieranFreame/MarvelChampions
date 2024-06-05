using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// "Cannot take damage while Legions of Hydra is in play. After Madame Hydra activates, place 2 threat on Legions of Hydra"
/// </summary>

[CreateAssetMenu(fileName = "Madame Hydra", menuName = "MarvelChampions/Card Effects/Legions of Hydra/Madame Hydra")]
public class MadameHydra : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as MinionCard).CharStats.Health.Modifiers.Add(ModifyDamage);

        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
        SchemeSystem.Instance.SchemeComplete.Add(IsTriggerMet);

        return Task.CompletedTask;
    }

    #region AddThreatToLegions
    public void IsTriggerMet(Action action)
    {
        if (action.Owner is MinionCard)
        {
            if ((action.Owner as MinionCard) == (MinionCard)Card)
                EffectResolutionManager.Instance.ResolvingEffects.Push(this);
        }
    }

    public override Task Resolve()
    {
        SchemeCard card = ScenarioManager.sideSchemes.FirstOrDefault(x => x.CardName == "Legions Of Hydra");

        if (card != default)
            card.Threat.GainThreat(2);
        
        return Task.CompletedTask;
    }
    #endregion


    #region NoDamageWhenLegionsActive
    private Task<DamageAction> ModifyDamage(DamageAction action)
    {
        if (ScenarioManager.sideSchemes.Any(x => x.CardName == "Legions Of Hydra"))
        {
            action.Value = 0;
        }

        return Task.FromResult(action);
    }
    #endregion

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(ModifyDamage);

        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
        SchemeSystem.Instance.SchemeComplete.Remove(IsTriggerMet);

        return Task.CompletedTask;
    }
}
