using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Cosmic Flight", menuName = "MarvelChampions/Card Effects/Captain Marvel/Cosmic Flight")]
public class CosmicFlight : PlayerCardEffect, IModifyDamage
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;
        _card = card;

        if (!_owner.Identity.IdentityTraits.Contains("Aerial"))
            _owner.Identity.IdentityTraits.Add("Aerial");

        DamageSystem.instance.Modifiers.Add(this);
    }

    /// <summary>
    /// When Captain Marvel would take damage, discard Cosmic Flight; prevent 3 of that damage;
    /// </summary>
    
    public IEnumerator OnTakeDamage(DamageAction action, System.Action<DamageAction> callback)
    {
        if (action.DamageTargets.Contains(_owner))
        {
            yield return _card.StartCoroutine(ConfirmActivateUI.MakeChoice(_card, decision =>
            {
                if (decision)
                {
                    action.Value -= 3;
                    _owner.Identity.IdentityTraits.Remove("Aerial");
                    DamageSystem.instance.Modifiers.Remove(this);
                    _owner.Deck.Discard(_card);
                }
            }));
        }

        callback(action);
    }
}
