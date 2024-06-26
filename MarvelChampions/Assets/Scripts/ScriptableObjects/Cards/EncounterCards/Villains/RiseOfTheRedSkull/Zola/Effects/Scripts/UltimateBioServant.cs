using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultimate Bio-Servant", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Ultimate Bio-Servant")]
public class UltimateBioServant : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        (_card as MinionCard).CharStats.Health.Tough = true;
        (_card as MinionCard).Attachments.CollectionChanged += ChangeAttack;

        return Task.CompletedTask;
    }

    private void ChangeAttack(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                (Card as MinionCard).CharStats.Attacker.CurrentAttack++;
                break;
            case NotifyCollectionChangedAction.Remove:
                (Card as MinionCard).CharStats.Attacker.CurrentAttack--;
                break;
        }
    }

    public override Task WhenDefeated()
    {
        (Card as MinionCard).Attachments.CollectionChanged -= ChangeAttack;
        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        ScenarioManager.inst.ActiveVillain.CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
