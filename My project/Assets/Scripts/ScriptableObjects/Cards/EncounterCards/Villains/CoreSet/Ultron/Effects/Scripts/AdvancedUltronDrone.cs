using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Advanced Ultron Drone", menuName = "MarvelChampions/Card Effects/Ultron/Advanced Ultron Drone")]
public class AdvancedUltronDrone : EncounterCardEffect
{
    Guard guard;
    UltronDrones ultronDrones;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        guard = new(Card as MinionCard);

        ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ultronDrones.SpawnDrone(TurnManager.instance.CurrPlayer);
        return Task.CompletedTask;
    }
}
