using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "Charge", menuName = "MarvelChampions/Card Effects/Rhino/Charge")]
public class Charge : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        _owner.CharStats.Attacker.CurrentAttack += 3;
        _owner.CharStats.Attacker.Keywords.Add(Keywords.Overkill);

        _owner.CharStats.AttackInitiated += AttackInitiated;
        await Task.Yield();
    }

    private void AttackInitiated() => AttackSystem.Instance.OnAttackCompleted.Add(AttackComplete);

    private Task AttackComplete(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackComplete);
        _owner.CharStats.AttackInitiated -= AttackInitiated;
        _owner.CharStats.Attacker.CurrentAttack -= 3;
        _owner.CharStats.Attacker.Keywords.Remove(Keywords.Overkill);
        ScenarioManager.inst.EncounterDeck.Discard(Card);

        return Task.CompletedTask;
    }
}
