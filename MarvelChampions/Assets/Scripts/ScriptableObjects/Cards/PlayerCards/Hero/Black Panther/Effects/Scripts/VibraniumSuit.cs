using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Vibranium Suit", menuName = "MarvelChampions/Card Effects/Black Panther/Vibranium Suit")]
public class VibraniumSuit : PlayerCardEffect, IBlackPanther
{
    bool attackSuccessful = false;

    public async Task Special(bool last)
    {
        if (!_owner.CharStats.Health.Damaged())
            return;

        _owner.CharStats.AttackInitiated += AttackInitiated;
        await _owner.CharStats.InitiateAttack(new(last ? 2 : 1, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner, card: Card));
        
        if (attackSuccessful)
        {
            _owner.CharStats.Health.CurrentHealth += last ? 2 : 1;
        }

        attackSuccessful = false;
        _owner.CharStats.AttackInitiated -= AttackInitiated;
    }

    private void AttackInitiated() { attackSuccessful = true; }
}
