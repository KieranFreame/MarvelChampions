using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSet
{
    [CreateAssetMenu(fileName = "Radioactive Man (Boost)", menuName = "MarvelChampions/Card Effects/Masters of Evil/Radioactive Man (Boost)")]
    public class RadioactiveManBoost : EncounterCardEffect
    {
        public override Task Resolve()
        {
            Player p = TurnManager.instance.CurrPlayer;

            Debug.Log("Discarding 1 Card from your hand");
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            Debug.Log("Discarded " + pCard.CardName);

            p.Hand.Discard(pCard);

            return Task.CompletedTask;
        }
    }
}

