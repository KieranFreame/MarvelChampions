using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hunted By Hydra", menuName = "MarvelChampions/Card Effects/RotRS/Taskmaster/Hunted By Hydra")]
public class HuntedByHydra : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        ScenarioManager.inst.MainScheme.Threat.GainThreat(1); //Incite 1

        foreach (var p in TurnManager.Players.Where(x => x.Identity.ActiveIdentity is Hero))
        {
            p.CharStats.Health.TakeDamage(new(p, 1));

            var pCard = p.Hand.cards[Random.Range(0, p.Hand.cards.Count)];

            p.Hand.Discard(pCard);
        }

        return Task.CompletedTask;
    }
}
