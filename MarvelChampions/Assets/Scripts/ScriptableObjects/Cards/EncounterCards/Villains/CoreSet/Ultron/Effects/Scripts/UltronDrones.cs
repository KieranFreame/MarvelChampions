using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultron Drones", menuName = "MarvelChampions/Card Effects/Ultron/Ultron Drones")]
public class UltronDrones : EncounterCardEffect
{
    public async void SpawnDrone(Player player)
    {
        MinionCardData droneData = Database.GetCardDataById("ULTRON-M-000") as MinionCardData;

        MinionCard drone = CreateCardFactory.Instance.CreateCard(droneData, RevealEncounterCardSystem.Instance.MinionTransform) as MinionCard;
        VillainTurnController.instance.MinionsInPlay.Add(drone);
        await drone.Effect.OnEnterPlay();

        PlayerCardData data = player.Deck.DealCard() as PlayerCardData;
        player.Deck.limbo.Remove(data);

        (drone.Effect as Drone).HijackCard(data, player);
    }
}
