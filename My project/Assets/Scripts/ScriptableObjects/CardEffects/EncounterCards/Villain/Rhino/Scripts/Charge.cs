using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "MarvelChampions/Card Effects/Rhino/Charge")]
public class Charge : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        _card = card;

        _owner.CharStats.Attacker.CurrentAttack += 3;
        _owner.Keywords.Add(Keywords.Overkill);

        AttackSystem.OnAttackComplete += AttackComplete;
    }

    private void AttackComplete(Action action)
    {
        if (action.Owner == null || action.Owner.GetType() is not Villain)
            return;

        AttackSystem.OnAttackComplete -= AttackComplete;
        _owner.CharStats.Attacker.CurrentAttack -= 3;
        _owner.Keywords.Remove(Keywords.Overkill);
        _owner.EncounterDeck.Discard(_card);
    }
}
