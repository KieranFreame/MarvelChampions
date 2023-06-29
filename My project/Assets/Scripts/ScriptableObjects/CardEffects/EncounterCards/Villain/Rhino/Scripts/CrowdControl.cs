using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Crowd Control", menuName = "MarvelChampions/Card Effects/Rhino/Crowd Control")]
public class CrowdControl : EncounterCardEffect
{
    /// <summary>
    /// Crisis Icon: While this scheme is in play, you cannot remove threat from the main scheme.
    /// </summary>

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        card.GetComponent<Threat>().WhenDefeated += WhenDefeated;
        TargetSystem.CheckPatrolAndCrisis += Crisis;

        await Task.Yield();
    }

    private void Crisis(List<Threat> candidates)
    {
        candidates.RemoveAll(x => x.GetComponent<MainSchemeCard>() != null);
    }

    public override void WhenDefeated()
    {
        Card.GetComponent<Threat>().WhenDefeated -= WhenDefeated;
        TargetSystem.CheckPatrolAndCrisis -= Crisis;
    }
}
