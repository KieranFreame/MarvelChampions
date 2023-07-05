using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Armored Rhino Suit", menuName = "MarvelChampions/Card Effects/Rhino/Armored Rhino Suit")]
public class ArmoredRhinoSuit : EncounterCardEffect, IModifyDamage
{
    private Counters counters;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;

        counters = Card.gameObject.AddComponent<Counters>();
        _owner.CharStats.Health.Modifiers.Add(this);
        await Task.Yield();
    }

    public async Task<int> OnTakeDamage(int damage)
    {
        counters.AddCounters(damage);

        if (counters.CountersLeft >= 5)
        {
            _owner.CharStats.Health.Modifiers.Remove(this);
            ScenarioManager.inst.EncounterDeck.Discard(Card);
        }

        await Task.Yield();
        return 0;
    }
}
