using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Drone Factory", menuName = "MarvelChampions/Card Effects/Ultron/Drone Factory")]
public class DroneFactory : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;

        foreach (var p in TurnManager.Players)
            ultronDrones.SpawnDrone(p);

        int threat = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).Count();
        (card as SchemeCard).Threat.GainThreat(threat);

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ScenarioManager.inst.MainScheme.Threat.Acceleration--;

        return Task.CompletedTask;
    }
}
