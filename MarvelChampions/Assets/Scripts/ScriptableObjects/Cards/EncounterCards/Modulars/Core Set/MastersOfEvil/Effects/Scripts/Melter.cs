using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Melter's attacks must be defended by an ally, if possible.
/// </summary>

[CreateAssetMenu(fileName = "Melter", menuName = "MarvelChampions/Card Effects/Masters of Evil/Melter")]
public class Melter : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        DefendSystem.Instance.OnSelectingDefender += SelectingDefender;

        return Task.CompletedTask;
    }

    #region MainEffect
    private void SelectingDefender(Player player, AttackAction action)
    {
        if (action.Card.CardName != "Melter")
            return;

        if (player.CardsInPlay.Allies.Count > 0)
        {
            CancelButton.CanEnable = false;
            DefendSystem.Instance.candidates.Remove(player);
            AttackSystem.Instance.OnAttackCompleted.Add(AttackCompleted);
        }
    }

    private void AttackCompleted(Action action)
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(AttackCompleted);
        CancelButton.CanEnable = true;
    }
    #endregion

    #region Boost
    public override Task Boost(Action action)
    {
        foreach (var a in TurnManager.instance.CurrPlayer.CardsInPlay.Allies)
            a.Exhaust();

        return Task.CompletedTask;
    }
    #endregion

    public override Task WhenDefeated()
    {
        DefendSystem.Instance.OnSelectingDefender -= SelectingDefender;

        return Task.CompletedTask;
    }
}
