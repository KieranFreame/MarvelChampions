using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crowd Control", menuName = "MarvelChampions/Card Effects/Rhino/Crowd Control")]
public class CrowdControl : CardEffect
{
    Threat mainScheme;

    /// <summary>
    /// Crisis Icon: While this scheme is in play, you cannot remove threat from the main scheme.
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        _card = card;

        mainScheme = FindObjectOfType<MainSchemeCard>().GetComponent<Threat>();
        _card.GetComponent<Threat>().WhenDefeated += WhenDefeated;
        TargetSystem.CheckPatrolAndCrisis += Crisis;
    }

    private void Crisis(List<Threat> candidates)
    {
        candidates.RemoveAll(x => x == mainScheme);
    }

    public override void WhenDefeated()
    {
        _card.GetComponent<Threat>().WhenDefeated -= WhenDefeated;
        TargetSystem.CheckPatrolAndCrisis -= Crisis;
    }
}
