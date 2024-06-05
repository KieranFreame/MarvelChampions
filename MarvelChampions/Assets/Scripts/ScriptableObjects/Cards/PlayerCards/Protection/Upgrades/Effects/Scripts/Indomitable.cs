using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Indomitable", menuName = "MarvelChampions/Card Effects/Protection/Upgrades/Indomitable")]
public class Indomitable : PlayerCardEffect, IOptional
{
    public override async Task OnEnterPlay()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
        await Task.Yield();
    }

    private void DefenderSelected(ICharacter target, AttackAction action)
    {
        if (target != _owner as ICharacter) return;

        EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override Task Resolve()
    {
        _owner.Ready();

        _owner.CardsInPlay.Permanents.Remove(_card);
        _owner.Deck.Discard(_card);
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;

        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
