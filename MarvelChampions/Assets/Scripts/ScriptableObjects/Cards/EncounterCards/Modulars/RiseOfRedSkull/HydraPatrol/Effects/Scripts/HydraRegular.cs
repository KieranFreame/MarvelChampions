using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Regular", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Hydra Patrol/Hydra Regular")]
public class HydraRegular : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
        return Task.CompletedTask;
    }
}
