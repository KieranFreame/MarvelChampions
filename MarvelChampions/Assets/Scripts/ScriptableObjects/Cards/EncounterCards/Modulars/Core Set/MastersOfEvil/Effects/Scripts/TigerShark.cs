using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tiger Shark", menuName = "MarvelChampions/Card Effects/Masters of Evil/Tiger Shark")]
public class TigerShark : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;
            return Task.CompletedTask;
    }

    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted); 

    private Task AttackCompleted(AttackAction action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);

        (Card as MinionCard).CharStats.Health.Tough = true;

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        action.Owner.CharStats.Health.Tough = true; 
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.AttackInitiated -= AttackInitiated;

        return Task.CompletedTask;
    }
}
