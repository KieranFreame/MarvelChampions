using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Android Efficiency", menuName = "MarvelChampions/Card Effects/Ultron/Android Efficiency")]
public class AndroidEfficiency : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;

        foreach (var p in TurnManager.Players)
            ultronDrones.SpawnDrone(p);

        return Task.CompletedTask;
    }

    public override async Task Boost(Action action)
    {
        Player p;

        if (action is AttackAction)
        {
            p = (action as AttackAction).Target is Player ? (action as AttackAction).Target as Player : ((action as AttackAction).Target as AllyCard).Owner;
        }
        else
        {
            p = TurnManager.instance.CurrPlayer;
        }

        if (p.HaveResource(Resource.Energy))
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Spend an Energy Resource", "Spawn a Drone" });

            if (decision == 1)
            {
                await PayCostSystem.instance.GetResources(new() { { Resource.Energy, 1 } });
                return;
            }
        }

        UltronDrones ultronDrones = GameObject.Find("Ultron Drones").GetComponent<EncounterCard>().Effect as UltronDrones;
        ultronDrones.SpawnDrone(p);
    }
}
