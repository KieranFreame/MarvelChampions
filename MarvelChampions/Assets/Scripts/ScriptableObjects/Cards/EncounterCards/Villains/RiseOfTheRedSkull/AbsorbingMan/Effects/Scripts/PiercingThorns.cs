using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Piercing Thorns", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Piercing Thorns")]
public class PiercingThorns : EncounterCardEffect 
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        var pCard = player.Hand.cards[Random.Range(0, player.Hand.cards.Count)];

        Debug.Log("Discarding " + pCard.CardName);

        player.Hand.Remove(pCard);
        player.Deck.Discard(pCard);

        if (owner.VillainTraits.Contains("Wood"))
        {
            Debug.Log("Discard 1 Card you control");

            List<PlayerCard> cards = new List<PlayerCard>();
            cards.AddRange(player.CardsInPlay.Allies);
            cards.AddRange(player.CardsInPlay.Permanents);

            var discard = await TargetSystem.instance.SelectTarget(cards);

            if (discard is AllyCard) player.CardsInPlay.Allies.Remove(discard as AllyCard);
            else player.CardsInPlay.Permanents.Remove(discard);

            discard.Effect.OnExitPlay();

            player.Deck.Discard(discard);
        }
    }

    public override Task Boost(Action action)
    {
        if (ScenarioManager.inst.ActiveVillain.VillainTraits.Contains("Stone") || ScenarioManager.inst.ActiveVillain.VillainTraits.Contains("Wood"))
        {
            TurnManager.instance.CurrPlayer.CharStats.Attacker.Stunned = true;
        }

        return Task.CompletedTask;
    }
}
