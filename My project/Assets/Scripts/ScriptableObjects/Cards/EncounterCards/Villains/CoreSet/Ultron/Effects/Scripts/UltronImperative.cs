using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultron's Imperative", menuName = "MarvelChampions/Card Effects/Ultron/Ultron's Imperative")]
public class UltronImperative : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        Player p = TurnManager.instance.FirstPlayer;

        UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;

        for (int i = 0; i < 2; i++)
            ultronDrones.SpawnDrone(p);

        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
