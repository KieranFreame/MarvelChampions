using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Crimson Cowl", menuName = "MarvelChampions/Card Effects/Ultron/The Crimson Cowl")]
public class CrimsonCowl : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        EncounterCardData data = ScenarioManager.inst.EncounterDeck.Search("Ultron Drones") as EncounterCardData;

        EncounterCard ultronDrones = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("AttachmentTransform").transform) as EncounterCard;
        await ultronDrones.OnRevealCard();

        UltronDrones eff = ultronDrones.Effect as UltronDrones;

        foreach (var p in TurnManager.Players)
            eff.SpawnDrone(p);
    }
}
