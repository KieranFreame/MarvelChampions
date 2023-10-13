using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Usurp the Throne", menuName = "MarvelChampions/Card Effects/Nemesis/Black Panther/Usurp the Throne")]
public class UsurpThrone : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        VillainTurnController.instance.HazardCount++;
        await Task.Yield();
    }

    public override Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;

        return Task.CompletedTask;
    }
}
