using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Omni-Morph Duplication", menuName = "MarvelChampions/Card Effects/RotRS/Absorbing Man/Omni-Morph Duplication")]
public class OmniMorphDuplication : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (owner.VillainTraits.Contains("Ice"))
            player.Exhaust();

        if (owner.VillainTraits.Contains("Metal"))
        {
            owner.CharStats.Health.Tough = true;
            owner.CharStats.Health.CurrentHealth += 1;
        }

        if (owner.VillainTraits.Contains("Stone"))
            BoostSystem.Instance.BoostCardCount++;

        if (owner.VillainTraits.Contains("Wood"))
        {
            var pCard = player.Hand.cards[Random.Range(0, player.Hand.cards.Count)];

            Debug.Log("Discarding " + pCard.CardName);

            player.Hand.Remove(pCard);
            player.Deck.Discard(pCard);
        }

        return Task.CompletedTask;
    }
}
