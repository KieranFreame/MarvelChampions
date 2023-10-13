using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Assault On NORAD", menuName = "MarvelChampions/Card Effects/Ultron/Assault On NORAD")]
public class AssaultOnNORAD : EncounterCardEffect
{
    UltronDrones ultronDrones;

    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;
        foreach (var p in TurnManager.Players)
            ultronDrones.SpawnDrone(p);

        (Card as MainSchemeCard).AfterStepOne.Add(Execute);

        return Task.CompletedTask;
    }

    public async Task Execute()
    {
        foreach (var p in TurnManager.Players)
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Place 2 threat on the Main Scheme", "Spawn a Drone" });

            if (decision == 1)
            {
                (Card as MainSchemeCard).Threat.GainThreat(2);
            }
            else
            {
                ultronDrones.SpawnDrone(p);
            }
        }
    }

    public override Task WhenDefeated()
    {
        (Card as MainSchemeCard).AfterStepOne.Remove(Execute);
        return Task.CompletedTask;
    }
}
