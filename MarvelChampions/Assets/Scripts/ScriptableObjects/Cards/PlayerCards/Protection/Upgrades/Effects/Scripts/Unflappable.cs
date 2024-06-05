using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Unflappable", menuName = "MarvelChampions/Card Effects/Protection/Upgrades/Unflappable")]
public class Unflappable : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == _card.CardName))
                return false;

            return true;
        }

        return false;
    }

    public override Task OnEnterPlay()
    {
        DefendSystem.Instance.OnDefenderSelected += DefenderSelected;
        return Task.CompletedTask;
    }

    private void DefenderSelected(ICharacter target, AttackAction action)
    {
        if (_card.Exhausted || target != _owner as ICharacter) return;

        _owner.CharStats.Health.OnTakeDamage += IsTriggerMet;
    }

    public override Task Resolve()
    {
        _card.Exhaust();
        DrawCardSystem.Instance.DrawCards(new(1, _owner));

        return Task.CompletedTask;
    }

    private void IsTriggerMet(DamageAction damage)
    {
        _owner.CharStats.Health.OnTakeDamage -= IsTriggerMet;

        if (damage.Value == 0)
            EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override void OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
