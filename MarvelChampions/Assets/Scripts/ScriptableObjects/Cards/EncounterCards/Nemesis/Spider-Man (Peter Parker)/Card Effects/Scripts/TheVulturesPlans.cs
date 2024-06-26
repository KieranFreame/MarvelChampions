using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "The Vulture's Plans", menuName = "MarvelChampions/Card Effects/Nemesis/Spider-Man (Peter Parker)/The Vulture's Plans")]
public class TheVulturesPlans : EncounterCardEffect
{
    public override Task Resolve()
    {
        HashSet<Resource> resources = new();

        foreach (Player p in TurnManager.Players)
        {
            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            foreach (Resource r in pCard.Resources)
                resources.Add(r);

            p.Hand.Discard(pCard);
        }

        if (resources.Count > 0)
        {
            ScenarioManager.inst.MainScheme.Threat.GainThreat(resources.Count);
        }

        return Task.CompletedTask;
    }
}
