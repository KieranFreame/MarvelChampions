using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussive Blast", menuName = "MarvelChampions/Card Effects/Under Attack/Concussive Blast")]
public class ConcussiveBlast : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        foreach (var p in TurnManager.Players)
        {
            p.CharStats.Health.TakeDamage(new(p, 1, card:card, owner:owner));

            for (int i = p.CardsInPlay.Allies.Count -1; i >= 0; i--)
                p.CardsInPlay.Allies[i].CharStats.Health.TakeDamage(new(p.CardsInPlay.Allies[i], 1, card: card, owner: owner));
        }

        return Task.CompletedTask;
    }

    public override Task Boost(Action action)
    {
        Player p;

        if (action is AttackAction)
        {
            var attack = action as AttackAction;
            p = (attack.Target is Player) ? attack.Target as Player : (attack.Target as AllyCard).Owner;
        }
        else
        {
            p = TurnManager.instance.CurrPlayer;
        }

        p.CharStats.Health.TakeDamage(new(p, 1, card: GameObject.Find("Concussive Blast").GetComponent<EncounterCard>(), owner: action.Owner));

        foreach (var a in p.CardsInPlay.Allies)
            a.CharStats.Health.TakeDamage(new(a, 1, card: GameObject.Find("Concussive Blast").GetComponent<EncounterCard>(), owner: action.Owner));

        return Task.CompletedTask;
    }
}
