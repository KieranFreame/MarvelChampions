using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Personal Challenge", menuName = "MarvelChampions/Card Effects/Nemesis/She-Hulk/Personal Challenge")]
public class PersonalChallenge : EncounterCardEffect
{
    Crisis _crisis;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        (card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);

        _crisis = new(card as SchemeCard);
        return Task.CompletedTask;
    }
}
