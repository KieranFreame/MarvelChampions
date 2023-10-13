using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hard To Keep Down", menuName = "MarvelChampions/Card Effects/Rhino/Hard To Keep Down")]
public class HardToKeepDown : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        var health = owner.CharStats.Health;

        if (!health.Damaged())
        {
            ScenarioManager.inst.Surge(player);
            return;
        }

        health.RecoverHealth(4);
        await Task.Yield();
    }
}
