using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Madame Hydra", menuName = "MarvelChampions/Card Effects/Legions of Hydra/Madame Hydra")]
public class MadameHydra : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as MinionCard).CharStats.Health.Modifiers.Add(ModifyDamage);

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
        (Card as MinionCard).CharStats.SchemeInitiated += SchemeInitiated;

        return Task.CompletedTask;
    }

    private void SchemeInitiated() => SchemeSystem.Instance.SchemeComplete.Add(ActivationCompleted);
    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(ActivationCompleted);

    private Task ActivationCompleted(Action action)
    {
        if (action is AttackAction)
        {
            AttackSystem.Instance.OnAttackCompleted.Remove(ActivationCompleted);
        }
        else
        {
            SchemeSystem.Instance.SchemeComplete.Remove(ActivationCompleted);
        }

        SchemeCard card = ScenarioManager.sideSchemes.FirstOrDefault(x => x.CardName == "Legions Of Hydra");

        if (card != default)
            card.Threat.GainThreat(2);

        return Task.CompletedTask;
    }

    private Task<DamageAction> ModifyDamage(DamageAction action)
    {
        if (ScenarioManager.sideSchemes.Any(x => x.CardName == "Legions Of Hydra"))
        {
            action.Value = 0;
        }

        return Task.FromResult(action);
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.Health.Modifiers.Remove(ModifyDamage);

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
        (Card as MinionCard).CharStats.SchemeInitiated -= SchemeInitiated;

        return Task.CompletedTask;
    }
}
