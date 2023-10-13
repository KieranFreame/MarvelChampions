using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Illegal Arms Factory", menuName = "MarvelChampions/Card Effects/Klaw/Illegal Arms Factory")]
public class IllegalArmsFactory : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        VillainTurnController.instance.HazardCount++;

        (card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);

        await Task.Yield();
    }

    public override async Task WhenDefeated()
    {
        VillainTurnController.instance.HazardCount--;
        await Task.Yield();
    }
}
