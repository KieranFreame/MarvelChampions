using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Abandoned Facility", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Abandoned Facility")]
public class AbandonedFacility : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.Surge(player);
        await OnEnterPlay(owner, card, player);
    }

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        _owner.CharStats.AttackInitiated += AttackInitiated;

        return Task.CompletedTask;
    }

    private void AttackInitiated()
    {
        DefendSystem.Instance.OnDefenderSelected += OnDefenderSelected;
    }

    private void OnDefenderSelected(ICharacter arg0)
    {
        DefendSystem.Instance.OnDefenderSelected -= OnDefenderSelected;

        if (arg0 == null)
        {
            AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete);
        }
    }

    private async Task AttackComplete(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackComplete);
        await PayCostSystem.instance.GetResources(Resource.Any, (NoneShallPass.delay.CountersLeft >= 5) ? 2 : 1);
    }

    public override async Task Boost(Action action)
    {
        var card = GameObject.Find("EncounterCards").transform.Find("Abandoned Facility").GetComponent<EncounterCard>();

        card.InPlay = true;
        RevealEncounterCardSystem.Instance.MoveCard(card);
        await OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, null);

        (ScenarioManager.inst.MainScheme.Effect as NoneShallPass).EncounterCardRevealed(card);
    }

    public override Task OnExitPlay()
    {
        _owner.CharStats.AttackInitiated -= AttackInitiated;

        return Task.CompletedTask;
    }
}
