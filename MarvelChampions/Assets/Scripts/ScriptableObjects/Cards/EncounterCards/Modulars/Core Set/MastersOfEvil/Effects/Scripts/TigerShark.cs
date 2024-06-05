using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Main Effect: After Tiger Shark attacks, he gains Tough.
/// Boost Effect: The villain gains Tough
/// </summary>

[CreateAssetMenu(fileName = "Tiger Shark", menuName = "MarvelChampions/Card Effects/Masters of Evil/Tiger Shark")]
public class TigerShark : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
        return Task.CompletedTask;
    }

    private void AttackCompleted(AttackAction action)
    {
        if (action.Card != null && action.Card.CardName == "Tiger Shark")
            if (((MinionCard)Card).CharStats.Health.Tough)
                return;

        ((MinionCard)Card).CharStats.Health.Tough = true;
    }

    public override Task Boost(Action action)
    {
       ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough = true; 
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);
        return Task.CompletedTask;
    }
}
