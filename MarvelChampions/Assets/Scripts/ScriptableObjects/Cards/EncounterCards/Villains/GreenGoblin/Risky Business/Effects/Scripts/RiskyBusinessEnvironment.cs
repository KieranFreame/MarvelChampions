using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Criminal Enterprise/State of Madness", menuName = "MarvelChampions/Card Effects/Risky Business/Criminal Enterprise|State of Madness")]
public class RiskyBusinessEnvironment : EncounterCardEffect
{
    Counters counters;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(2 * TurnManager.Players.Count);

        RiskyBusiness.Instance.environment = this;

        return Task.CompletedTask;
    }

    public void AddCounters(int count) => counters.AddCounters(count);

    public void RemoveCounters(int count)
    {
        counters.RemoveCounters(count);

        if (counters.CountersLeft <= 0)
        {
            RiskyBusiness.Instance.Flip();
            counters.CountersLeft = 2 * TurnManager.Players.Count;
        }
    }

    public int GetCounters() => counters.CountersLeft;
}
