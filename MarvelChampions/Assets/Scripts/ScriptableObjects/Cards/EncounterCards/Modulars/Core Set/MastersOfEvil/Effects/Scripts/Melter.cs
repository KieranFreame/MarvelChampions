using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Melter", menuName = "MarvelChampions/Card Effects/Masters of Evil/Melter")]
public class Melter : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        (Card as MinionCard).CharStats.AttackInitiated += AttackInitiated;

        return Task.CompletedTask;
    }

    private void AttackInitiated() => DefendSystem.Instance.OnSelectingDefender += SelectingDefender;
    

    private void SelectingDefender(Player player)
    {
        DefendSystem.Instance.OnSelectingDefender -= SelectingDefender;

        if (player.CardsInPlay.Allies.Count > 0)
        {
            CancelButton.CanEnable = false;
            DefendSystem.Instance.candidates.Remove(player);
        }

        AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
    }

    private Task AttackCompleted(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);
        CancelButton.CanEnable = true;
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        Player p;

        if (action is AttackAction)
        {
            var attack = action as AttackAction;
            p = (attack.Target is not Player) ? (attack.Target as AllyCard).Owner : attack.Target as Player;
        }
        else //scheme
        {
            p = TurnManager.instance.CurrPlayer;
        }

        foreach (var a in p.CardsInPlay.Allies)
            a.Exhaust();

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).CharStats.AttackInitiated -= AttackInitiated;

        return Task.CompletedTask;
    }
}
