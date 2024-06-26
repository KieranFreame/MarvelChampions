using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Main Effect: Quickstrike (When this minion enters play, it attacks)
/// Boost: If you are in Hero form, the villain attacks (with no boost cards) after this activation.
/// </summary>

[CreateAssetMenu(fileName = "Hydra Jet-Trooper", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Assault/Hydra Jet-Trooper")]
public class HydraJetTrooper : EncounterCardEffect
{
    public override async Task OnEnterPlay()
    {
        await (_card as MinionCard).CharStats.InitiateAttack();
    }

    public override Task Boost(Action action)
    {
        if (TurnManager.instance.CurrPlayer.Identity.ActiveIdentity is not Hero)
            EffectManager.Inst.Resolving.Push(this);

        return Task.CompletedTask;
    }

    public override async Task Resolve()
    {
        BoostSystem.Instance.BoostCardCount = 0;
        await ScenarioManager.inst.ActiveVillain.CharStats.InitiateAttack();
    }
}
