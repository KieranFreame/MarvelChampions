using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Corporate Acquisition", menuName = "MarvelChampions/Card Effects/Risky Business/Corporate Acquisition")]
public class CorporateAcquisition : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }
}
