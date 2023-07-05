using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "The Vulture's Plans", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/The Vulture's Plans")]
public class TheVulturesPlans : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        List<Resource> resources = new();

        foreach (Player p in TurnManager.Players)
        {
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            if (!resources.Any(x => pCard.Resources.Contains(x)))
            {
                resources.AddRange(pCard.Resources);
            }

            p.Hand.Remove(pCard);
            p.Deck.Discard(pCard);
        }

        if (resources.Count > 0)
        {
            FindObjectOfType<MainSchemeCard>().GetComponent<Threat>().GainThreat(resources.Count);
        }

        await Task.Yield();
    }
}
