using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Taskmaster's Training Camp", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Taskmaster's Training Camp")]
public class TrainingCamp : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        VillainTurnController.instance.MinionsInPlay.CollectionChanged += ApplyTough;
        VillainTurnController.instance.HazardCount++;

        (card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;

        return Task.CompletedTask;
    }

    private void ApplyTough(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                (item as MinionCard).CharStats.Health.Tough = true;
            }
        }
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.MinionsInPlay.CollectionChanged -= ApplyTough;
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
