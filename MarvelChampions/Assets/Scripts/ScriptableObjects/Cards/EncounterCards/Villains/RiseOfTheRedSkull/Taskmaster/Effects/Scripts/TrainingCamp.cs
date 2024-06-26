using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Taskmaster's Training Camp", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Taskmaster's Training Camp")]
public class TrainingCamp : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        RevealEncounterCardSystem.OnEncounterCardRevealed += ApplyTough;
        VillainTurnController.instance.HazardCount++;

        (_card as SchemeCard).Threat.CurrentThreat *= TurnManager.Players.Count;

        return Task.CompletedTask;
    }

    private void ApplyTough(EncounterCard card)
    {
        if (card is MinionCard)
        {
            (card as MinionCard).CharStats.Health.Tough = true;
        }
    }

    public override Task WhenDefeated()
    {
        RevealEncounterCardSystem.OnEncounterCardRevealed -= ApplyTough;
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
