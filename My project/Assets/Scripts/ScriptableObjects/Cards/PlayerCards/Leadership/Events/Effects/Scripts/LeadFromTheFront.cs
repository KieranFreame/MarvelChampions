using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Lead From The Front", menuName = "MarvelChampions/Card Effects/Leadership/Lead From The Front")]
public class LeadFromTheFront : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        _owner.CharStats.Thwarter.CurrentThwart++;

        foreach (AllyCard a in _owner.CardsInPlay.Allies)
        {
            a.CharStats.Attacker.CurrentAttack++;
            a.CharStats.Thwarter.CurrentThwart++;
        }

        _owner.CardsInPlay.Allies.CollectionChanged += AlliesChanged;
        TurnManager.OnEndPlayerPhase += OnEndPhase;

        return Task.CompletedTask;
    }

    private void AlliesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (AllyCard a in e.NewItems)
            {
                a.CharStats.Attacker.CurrentAttack++;
                a.CharStats.Thwarter.CurrentThwart++;
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (AllyCard a in e.OldItems)
            {
                a.CharStats.Attacker.CurrentAttack--;
                a.CharStats.Thwarter.CurrentThwart--;
            }
        }
    }

    private void OnEndPhase()
    {
        _owner.CardsInPlay.Allies.CollectionChanged -= AlliesChanged;
        TurnManager.OnEndPlayerPhase -= OnEndPhase;

        _owner.CharStats.Attacker.CurrentAttack--;
        _owner.CharStats.Thwarter.CurrentThwart--;

        foreach (AllyCard a in _owner.CardsInPlay.Allies)
        {
            a.CharStats.Attacker.CurrentAttack--;
            a.CharStats.Thwarter.CurrentThwart--;
        }
    }
}
