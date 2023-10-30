using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mutagen Cloud", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Mutagen Cloud")]
public class MutagenCloud : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        VillainTurnController.instance.MinionsInPlay.CollectionChanged += UpdateAcceleration;
        ScenarioManager.inst.MainScheme.Threat.Acceleration = 1 + VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Goblin")).Count();
        return Task.CompletedTask;
    }

    private void UpdateAcceleration(object sender, NotifyCollectionChangedEventArgs e)
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration = 1 + VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Goblin")).Count();
    }
}
