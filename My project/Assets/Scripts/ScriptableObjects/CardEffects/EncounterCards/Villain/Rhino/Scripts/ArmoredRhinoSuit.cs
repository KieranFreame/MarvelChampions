using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Armored Rhino Suit", menuName = "MarvelChampions/Card Effects/Rhino/Armored Rhino Suit")]
public class ArmoredRhinoSuit : EncounterCardEffect, IModifyDamage
{
    private int damageTaken = 0;

    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        _card = card;

        DamageSystem.instance.Modifiers.Add(this);
    }

    public IEnumerator OnTakeDamage(DamageAction action, System.Action<DamageAction> callback)
    {
        if (action.DamageTargets.Contains(_owner))
        {
            damageTaken += action.Value;
            action.Value = 0;
            callback(action);

            if (damageTaken >= 5)
            {
                DamageSystem.instance.Modifiers.Remove(this);
                _owner.EncounterDeck.Discard(_card);
            }
        }

        yield break;
    }
}
