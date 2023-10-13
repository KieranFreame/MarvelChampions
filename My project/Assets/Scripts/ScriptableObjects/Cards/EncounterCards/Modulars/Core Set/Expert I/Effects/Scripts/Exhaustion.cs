using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Exhaustion", menuName = "MarvelChampions/Card Effects/Expert/Exhaustion")]
public class Exhaustion : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.Surge(player);

        player.Exhaust();

        return Task.CompletedTask;
    }
}
