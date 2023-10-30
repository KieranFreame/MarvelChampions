using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Overconfidence", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Overconfidence")]
public class Overconfidence : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is Hero)
        {
            owner.CharStats.AttackInitiated += AttackInitiated;
            await owner.CharStats.InitiateAttack();
            owner.CharStats.AttackInitiated -= AttackInitiated;
        }
        else
        {
            owner.CharStats.SchemeInitiated += SchemeInitiated;
            await owner.CharStats.InitiateScheme();
            owner.CharStats.SchemeInitiated -= SchemeInitiated;
        }
    }

    private void SchemeInitiated()
    {
        SchemeSystem.Instance.SchemeComplete.Add(SchemeComplete);
    }

    private Task SchemeComplete(SchemeAction schemeAction)
    {
        SchemeSystem.Instance.SchemeComplete.Remove(SchemeComplete);

        if (schemeAction.Value >= 3)
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        DefendSystem.Instance.OnTargetSelected += DefenderSelected;
    }

    private void DefenderSelected(ICharacter arg0)
    {
        DefendSystem.Instance.OnTargetSelected -= DefenderSelected;
        arg0.CharStats.Health.OnTakeDamage += ActivationComplete;
    }

    private void ActivationComplete(DamageAction arg0)
    {
        arg0.DamageTargets[0].CharStats.Health.OnTakeDamage -= ActivationComplete;

        if (arg0.Value >= 3)
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
    }
}
