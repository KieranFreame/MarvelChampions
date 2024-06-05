using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Snowy Hillside", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Snowy Hillside")]
public class SnowyHillside : EncounterCardEffect
{
    bool isBoost;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.Surge(player);
        await OnEnterPlay(owner, card, player);
    }

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        DefendSystem.Instance.OnDefenderSelected += OnDefenderSelected;

        return Task.CompletedTask;
    }

    private void OnDefenderSelected(ICharacter target, AttackAction action)
    {
        if (target == null)
        {
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
        }
    }

    public override async Task Resolve()
    {
        if (isBoost)
        {
            var card = GameObject.Find("EncounterCards").transform.Find("Snowy Hillside").GetComponent<EncounterCard>();

            card.InPlay = true;
            RevealEncounterCardSystem.Instance.MoveCard(card);
            await OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, null);

            (ScenarioManager.inst.MainScheme.Effect as NoneShallPass).EncounterCardRevealed(card);

            isBoost = false;
            return;
        }

        ScenarioManager.inst.MainScheme.Threat.GainThreat((NoneShallPass.delay.CountersLeft >= 5) ? 2 : 1);
    }

    public override Task Boost(Action action)
    {
        isBoost = true;
        EffectResolutionManager.Instance.ResolvingEffects.Push(this);

        return Task.CompletedTask;
    }

    public override Task OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= OnDefenderSelected;
        return Task.CompletedTask;
    }
}
