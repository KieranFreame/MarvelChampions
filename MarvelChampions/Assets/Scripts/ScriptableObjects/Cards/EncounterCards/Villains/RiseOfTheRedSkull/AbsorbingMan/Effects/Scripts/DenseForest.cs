using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Dense Forest", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Dense Forest")]
public class DenseForest : EncounterCardEffect
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

    private void OnDefenderSelected(ICharacter arg0, AttackAction action)
    {
        if (arg0 == null)
        {
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
        }
    }

    public override async Task Resolve()
    {
        if (isBoost)
        {
            var card = GameObject.Find("EncounterCards").transform.Find("Dense Forest").GetComponent<EncounterCard>();

            card.InPlay = true;
            RevealEncounterCardSystem.Instance.MoveCard(card);
            await OnEnterPlay(ScenarioManager.inst.ActiveVillain, card, null);

            (ScenarioManager.inst.MainScheme.Effect as NoneShallPass).EncounterCardRevealed(card);

            isBoost = false;

            return;
        }

        List<ICharacter> targets = new() { TurnManager.instance.CurrPlayer };
        targets.AddRange((targets[0] as Player).CardsInPlay.Allies);

        await IndirectDamageHandler.inst.HandleIndirectDamage(targets, (NoneShallPass.delay.CountersLeft >= 5) ? 2 : 1);
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
