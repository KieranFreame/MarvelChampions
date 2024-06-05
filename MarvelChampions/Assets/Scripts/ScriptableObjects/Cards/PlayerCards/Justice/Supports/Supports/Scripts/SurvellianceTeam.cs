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
        counters = _card.gameObject.AddComponent<Counters>();
        counters.AddCounters(3);

        await Task.Yield();
    }

    public override bool CanActivate()
    {
        return !_card.Exhausted && ScenarioManager.inst.ThreatPresent();
    }

    public override async Task Activate()
    {
        _card.Exhaust();
        counters.RemoveCounters(1);

        await ThwartSystem.Instance.InitiateThwart(new(1, null));

        if (counters.CountersLeft == 0)
        {
            _owner.CardsInPlay.Permanents.Remove(_card);
            _owner.Deck.Discard(_card);
        }
    }
}
