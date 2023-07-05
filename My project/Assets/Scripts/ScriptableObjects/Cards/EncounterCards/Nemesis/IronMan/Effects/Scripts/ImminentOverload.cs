using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Imminent Overload", menuName = "MarvelChampions/Card Effects/Nemesis/Iron Man/Imminent Overload")]
public class ImminentOverload : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        card.GetComponent<Threat>().GainThreat(1 * TurnManager.Players.Count);
        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration++;

        await Task.Yield();
    }

    public override void OnExitPlay()
    {
        FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().Acceleration--;
    }
}
