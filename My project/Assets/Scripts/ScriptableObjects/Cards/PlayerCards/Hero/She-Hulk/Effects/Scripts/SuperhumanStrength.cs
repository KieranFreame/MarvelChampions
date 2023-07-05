using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Superhuman Strength", menuName = "MarvelChampions/Card Effects/She-Hulk/Superhuman Strength")]
public class SuperhumanStrength : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack += 2;

        _owner.CharStats.AttackInitiated += AttackInitiated;

        await Task.Yield();
    }

    private void AttackInitiated()
    {
        AttackSystem.OnAttackComplete += AttackComplete;
    }

    private void AttackComplete(Action action)
    {
        AttackSystem.OnAttackComplete -= AttackComplete;
        var attack = action as AttackAction;

        if (attack.Target.CharStats.Health.CurrentHealth > 0)
        {
            attack.Target.CharStats.Attacker.Stunned = true;
        }

        _owner.CharStats.Attacker.CurrentAttack -= 2;
        _owner.CharStats.AttackInitiated -= AttackInitiated;

        _owner.CardsInPlay.Permanents.Remove(Card);
        _owner.Deck.Discard(Card);
    }
}
