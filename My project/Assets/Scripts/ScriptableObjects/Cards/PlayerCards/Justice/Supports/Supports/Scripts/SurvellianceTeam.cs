using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Survelliance Team", menuName = "MarvelChampions/Card Effects/Justice/Survelliance Team")]
public class SurvellianceTeam : PlayerCardEffect
{
    private Counters counters;

    public override async Task OnEnterPlay()
    {
        counters = Card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        counters.RemoveCounters(1);

        await ThwartSystem.instance.InitiateThwart(new(1));

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(Card);
            _owner.Deck.Discard(Card);
        }
    }
}
