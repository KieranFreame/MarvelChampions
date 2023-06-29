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
        DamageSystem.instance.Modifiers.Add(this);
        await Task.Yield();
    }

    public async Task<DamageAction> OnTakeDamage(DamageAction action, ICharacter target)
    {
        if (target == _owner as ICharacter)
        {
            counters.AddCounters(action.Value);
            action.Value = 0;

            if (counters.CountersLeft >= 5)
            {
                DamageSystem.instance.Modifiers.Remove(this);
                ScenarioManager.inst.EncounterDeck.Discard(Card);
            }
        }

        await Task.Yield();
        return action;
    }
}
