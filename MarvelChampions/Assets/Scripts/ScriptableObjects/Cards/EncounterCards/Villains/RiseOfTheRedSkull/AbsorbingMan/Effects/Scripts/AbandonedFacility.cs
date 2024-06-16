using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Abandoned Facility", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Abandoned Facility")]
public class AbandonedFacility : EncounterCardEffect
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
        _card = card;

        DefendSystem.Instance.OnDefenderSelected += OnDefenderSelected;

        return Task.CompletedTask;
    }

    private void OnDefenderSelected(ICharacter target, AttackAction action)
    {
        if (target == null)
        {
            EffectManager.Inst.Resolving.Push(this);
        }
    }

    public override async Task Resolve()
    {
        if (isBoost)
        {
            var card = GameObject.Find("EncounterCards").transform.Find("Abandoned Facility").GetComponent<EncounterCard>();

            card.InPlay = true;
            RevealEncounterCardSystem.Instance.MoveCard(card);
            await OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, null);

            (ScenarioManager.inst.MainScheme.Effect as NoneShallPass).EncounterCardRevealed(card);

            isBoost = false;
            return;
        }

        await PayCostSystem.instance.GetResources(Resource.Any, (NoneShallPass.delay.CountersLeft >= 5) ? 2 : 1);
    }

    public override Task Boost(Action action)
    {
        isBoost = true;
        EffectManager.Inst.Resolving.Push(this);

        return Task.CompletedTask;
    }

    public override Task OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= OnDefenderSelected;
        return Task.CompletedTask;
    }
}
