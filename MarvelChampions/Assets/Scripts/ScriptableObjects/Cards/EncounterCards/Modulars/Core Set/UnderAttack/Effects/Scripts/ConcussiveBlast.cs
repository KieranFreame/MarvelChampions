using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussive Blast", menuName = "MarvelChampions/Card Effects/Under Attack/Concussive Blast")]
public class ConcussiveBlast : EncounterCardEffect
{
    public override Task Resolve()
    {
        foreach (var p in TurnManager.Players)
        {
            p.CharStats.Health.TakeDamage(new(p, 1, false, _card, _owner));

            for (int i = p.CardsInPlay.Allies.Count -1; i >= 0; i--)
                p.CardsInPlay.Allies[i].CharStats.Health.TakeDamage(new(p.CardsInPlay.Allies[i], 1, false, _card, _owner));
        }

        return Task.CompletedTask;
    }
}
