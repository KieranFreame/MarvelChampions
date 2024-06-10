using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Armored Rhino Suit", menuName = "MarvelChampions/Card Effects/Rhino/Armored Rhino Suit")]
public class ArmoredRhinoSuit : EncounterCardEffect
{
    private Counters counters;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        counters = _card.gameObject.AddComponent<Counters>();
        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);
        await Task.Yield();
    }

    public Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        counters.AddCounters(action.Value);
        action.Value = 0;

        if (counters.CountersLeft >= 5)
        {
            _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        return Task.FromResult(action);
    }
}
