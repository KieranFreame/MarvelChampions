using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Cosmic Flight", menuName = "MarvelChampions/Card Effects/Captain Marvel/Cosmic Flight")]
public class CosmicFlight : PlayerCardEffect, IModifyDamage
{
    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _owner = owner;
        Card = card;

        if (!_owner.Identity.IdentityTraits.Contains("Aerial"))
            _owner.Identity.IdentityTraits.Add("Aerial");

        DamageSystem.instance.Modifiers.Add(this);

        await Task.Yield();
    }

    /// <summary>
    /// When Captain Marvel would take damage, discard Cosmic Flight; prevent 3 of that damage;
    /// </summary>
    
    public async Task<DamageAction> OnTakeDamage(DamageAction action, ICharacter target)
    {
        if (target == _owner as ICharacter)
        {
            bool decision = await ConfirmActivateUI.MakeChoice(Card);

            if (decision)
            {
                action.Value -= 3;
                _owner.Identity.IdentityTraits.Remove("Aerial");
                DamageSystem.instance.Modifiers.Remove(this);
                _owner.Deck.Discard(Card);
            }

        }

        return action;
    }
}
