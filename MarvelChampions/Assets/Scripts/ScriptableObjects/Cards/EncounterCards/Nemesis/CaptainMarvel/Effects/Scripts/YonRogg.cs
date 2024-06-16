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

        GameStateManager.Instance.OnActivationCompleted += CanRespond;

        await Task.Yield();
    }

    private async void CanRespond(Action action)
    {
        if (action is AttackAction && action.Owner.Name == "Yon-Rogg")
            await EffectManager.Inst.AddEffect(_card, this);
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
        GameStateManager.Instance.OnActivationCompleted -= CanRespond;
        return Task.CompletedTask;
    }
}
