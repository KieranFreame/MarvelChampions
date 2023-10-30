using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Swarm Attack", menuName = "MarvelChampions/Card Effects/Ultron/Swarm Attack")]
public class SwarmAttack : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        List<MinionCard> drones = VillainTurnController.instance.MinionsInPlay.Where(x => x.CardTraits.Contains("Drone")).ToList();

        if (drones.Count > 0)
        {
            foreach (var drone in drones)
            {
                await drone.CharStats.InitiateAttack();
            }
        }
        else
        {
            UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;
            ultronDrones.SpawnDrone(player);
        }
    }
}
