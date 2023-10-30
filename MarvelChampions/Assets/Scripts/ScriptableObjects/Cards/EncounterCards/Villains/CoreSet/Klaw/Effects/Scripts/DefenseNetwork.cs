using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Defense Network", menuName = "MarvelChampions/Card Effects/Klaw/Defense Network")]
public class DefenseNetwork : EncounterCardEffect
{
    Crisis _crisis;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        (Card as SchemeCard).Threat.GainThreat(1 * TurnManager.Players.Count);

        _crisis = new(Card as SchemeCard);

        return Task.CompletedTask;
    }
}
