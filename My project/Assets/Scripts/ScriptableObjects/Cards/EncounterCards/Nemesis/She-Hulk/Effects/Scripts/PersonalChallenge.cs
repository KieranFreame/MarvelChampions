using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Personal Challenge", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Personal Challenge")]
public class PersonalChallenge : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        card.GetComponent<Threat>().GainThreat(1 * TurnManager.Players.Count);

        TargetSystem.CheckPatrolAndCrisis += Crisis;

        await Task.Yield();
    }

    private void Crisis(List<Threat> candidates)
    {
        candidates.RemoveAll(x => x.GetComponent<MainSchemeCard>() != null);
    }

    public override async Task WhenDefeated()
    {
        TargetSystem.CheckPatrolAndCrisis -= Crisis;
        await Task.Yield();
    }
}
