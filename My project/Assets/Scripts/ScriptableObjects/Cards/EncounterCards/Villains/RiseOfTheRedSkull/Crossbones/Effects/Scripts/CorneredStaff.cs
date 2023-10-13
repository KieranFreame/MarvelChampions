using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Cornered Staff", menuName = "MarvelChampions/Card Effects/RotRS/Crossbones/Cornered Staff")]
public class CorneredStaff : EncounterCardEffect
{
    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;

        ThwartSystem.Instance.Crisis.Add(Card as SchemeCard);

        int threat = 0;

        for (int i = 1 * TurnManager.Players.Count; i > 0; i--)
        {
            threat += (ScenarioManager.inst.EncounterDeck.deck[0] as EncounterCardData).boostIcons;
            ScenarioManager.inst.EncounterDeck.Mill(1);
        }

        (Card as SchemeCard).Threat.GainThreat(threat);

        return Task.CompletedTask;
    }

    public override Task WhenDefeated()
    {
        ThwartSystem.Instance.Crisis.Remove(Card as SchemeCard);
        return Task.CompletedTask;
    }
}
