using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Breakin & Takin", menuName = "MarvelChampions/Card Effects/Rhino/Breakin n Takin")]
public class BreakinAndTakin : EncounterCardEffect
{
    /// <summary>
    /// When Revealed: Place an additional 1 per player threat here.
    /// (Hazard Icon: Deal +1 encounter ICard during the villain phase.)
    /// </summary>

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        card.GetComponent<Threat>().GainThreat(1 * TurnManager.Players.Count);
        VillainTurnController.instance.HazardCount++;
        await Task.Yield();
    }

    public override void OnExitPlay()
    {
        VillainTurnController.instance.HazardCount--;
    }
}
