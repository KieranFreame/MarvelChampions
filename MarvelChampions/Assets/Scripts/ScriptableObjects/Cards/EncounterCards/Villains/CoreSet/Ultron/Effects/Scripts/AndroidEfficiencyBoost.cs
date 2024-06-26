using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Android Efficiency (Boost)", menuName = "MarvelChampions/Card Effects/Ultron/Android Efficiency (Boost)")]
public class AndroidEfficiencyBoost : EncounterCardEffect
{
    public override async Task Resolve()
    {
        Player p = TurnManager.instance.CurrPlayer;

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
