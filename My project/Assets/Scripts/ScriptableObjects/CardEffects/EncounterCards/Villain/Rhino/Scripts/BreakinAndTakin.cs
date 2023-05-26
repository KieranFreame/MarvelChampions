using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Breakin & Takin", menuName = "MarvelChampions/Card Effects/Rhino/Breakin n Takin")]
public class BreakinAndTakin : CardEffect
{
    /// <summary>
    /// When Revealed: Place an additional 1 per player threat here.
    /// (Hazard Icon: Deal +1 encounter card during the villain phase.)
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        card.GetComponent<Threat>().GainThreat(1 * TurnManager.Players.Count);
        VillainTurnController.instance.HazardCount++;
    }

    public override void OnExitPlay()
    {
        VillainTurnController.instance.HazardCount--;
    }
}
