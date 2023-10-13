using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Countdown To Oblivion", menuName = "MarvelChampions/Card Effects/Ultron/Countdown To Oblivion")]
public class CountdownToOblivion : EncounterCardEffect
{
    Crisis _crisis;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;
        foreach (var p in TurnManager.Players)
            ultronDrones.SpawnDrone(p);

        _crisis = new(card as SchemeCard);

        return Task.CompletedTask;
    }
}
