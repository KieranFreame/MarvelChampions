using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Yon-Rogg", menuName = "MarvelChampions/Card Effects/Nemesis/Captain Marvel/Yon-Rogg")]
public class YonRogg : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        owner.CharStats.AttackInitiated += AttackInitiated;

        await Task.Yield();
    }

    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete);

    private Task AttackComplete(Action Attack)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackComplete);

        SchemeCard psychemagnitron = ScenarioManager.sideSchemes.FirstOrDefault(x => x.CardName == "The Psyche-Magnitron");

        if (psychemagnitron != default)
            psychemagnitron.Threat.GainThreat(1);

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;
        return Task.CompletedTask;
    }
}
