using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// After Yon-Rogg attacks, place 1 threat on The Psyche-Magnitron
/// </summary>

[CreateAssetMenu(fileName = "Yon-Rogg", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Yon-Rogg")]
public class YonRogg : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);

        await Task.Yield();
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Owner == Card as ICharacter)
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override Task Resolve()
    {
        SchemeCard psychemagnitron = ScenarioManager.sideSchemes.FirstOrDefault(x => x.CardName == "The Psyche-Magnitron");

        if (psychemagnitron != default)
            psychemagnitron.Threat.GainThreat(1);

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
        return Task.CompletedTask;
    }
}
