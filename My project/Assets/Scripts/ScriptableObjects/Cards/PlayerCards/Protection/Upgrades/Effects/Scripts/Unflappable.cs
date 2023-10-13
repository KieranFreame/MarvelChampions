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
            if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == Card.CardName))
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

    private void DefenderSelected(ICharacter target)
    {
        if (Card.Exhausted) return;

        if (target == _owner as ICharacter)
            _owner.CharStats.Health.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(DamageAction arg0)
    {
        _owner.CharStats.Health.OnTakeDamage -= OnTakeDamage;

        if (arg0.Value <= 0)
        {
            Card.Exhaust();
            DrawCardSystem.Instance.DrawCards(new(1, _owner));
        }
    }

    public override void OnExitPlay()
    {
        DefendSystem.Instance.OnDefenderSelected -= DefenderSelected;
    }
}
