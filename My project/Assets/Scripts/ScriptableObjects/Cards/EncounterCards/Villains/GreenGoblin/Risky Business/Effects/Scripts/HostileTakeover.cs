using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hostile Takeover", menuName = "MarvelChampions/Card Effects/Risky Business/Hostile Takeover")]
public class HostileTakeover : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        EncounterCard enterprise = CreateCardFactory.Instance.CreateCard(Database.GetCardDataById("RISKY-E-000"), GameObject.Find("AttachmentTransform").transform) as EncounterCard;
        await enterprise.OnRevealCard();
    }

    public override Task WhenCompleted()
    {
        RiskyBusiness.Instance.environment.AddCounters(1 * TurnManager.Players.Count);

        foreach (var player in TurnManager.Players)
        {
            player.Deck.Mill(RiskyBusiness.Instance.environment.GetCounters());
        }

        return Task.CompletedTask;
    }
}
