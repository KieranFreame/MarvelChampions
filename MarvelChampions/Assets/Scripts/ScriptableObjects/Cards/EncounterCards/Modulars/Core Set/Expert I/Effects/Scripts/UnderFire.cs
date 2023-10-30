using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Under Fire", menuName = "MarvelChampions/Card Effects/Expert/Under Fire")]
public class UnderFire : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        for (int i = 0; i < 1; i++)
        {
            ScenarioManager.inst.Surge(player);
        }

        return Task.CompletedTask;
    }
}
