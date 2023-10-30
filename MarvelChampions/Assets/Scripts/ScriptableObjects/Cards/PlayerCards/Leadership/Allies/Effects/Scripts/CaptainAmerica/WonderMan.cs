using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Wonder Man", menuName = "MarvelChampions/Card Effects/Leadership/Wonder Man")]
public class WonderMan : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        (Card as AllyCard).CharStats.Attacker.AttackCancel.Add(PayCost);
        _owner.Hand.cards.CollectionChanged += HandChanged;

        return Task.CompletedTask;
    }

    private void HandChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            (Card as AllyCard).CanAttack = true;
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (_owner.Hand.cards.Count == 0)
                (Card as AllyCard).CanAttack = false;
        }
    }

    private async Task<AttackAction> PayCost(AttackAction action)
    {
        PlayerCard discard = await TargetSystem.instance.SelectTarget(_owner.Hand.cards.ToList());
        _owner.Hand.Remove(discard);
        _owner.Deck.Discard(discard);

        return action;
    }

    public override void OnExitPlay()
    {
        (Card as AllyCard).CharStats.Attacker.AttackCancel.Remove(PayCost);
        _owner.Hand.cards.CollectionChanged -= HandChanged;
    }
}
