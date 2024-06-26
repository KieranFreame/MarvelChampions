using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Corporate Acquisition", menuName = "MarvelChampions/Card Effects/Risky Business/Corporate Acquisition")]
public class CorporateAcquisition : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        VillainTurnController.instance.HazardCount++;
        return Task.CompletedTask;
    }
}
