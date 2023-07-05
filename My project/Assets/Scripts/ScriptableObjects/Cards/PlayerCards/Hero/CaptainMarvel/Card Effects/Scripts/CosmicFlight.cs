using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Cosmic Flight", menuName = "MarvelChampions/Card Effects/Captain Marvel/Cosmic Flight")]
public class CosmicFlight : PlayerCardEffect, IModifyDamage
{
    public override async Task OnEnterPlay()
    {
        if (!_owner.Identity.IdentityTraits.Contains("Aerial"))
            _owner.Identity.IdentityTraits.Add("Aerial");

        _owner.CharStats.Health.Modifiers.Add(this);

        await Task.Yield();
    }

    public async Task<int> OnTakeDamage(int damage)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            damage -= 3;
            _owner.Identity.IdentityTraits.Remove("Aerial");
            _owner.CharStats.Health.Modifiers.Remove(this);
            _owner.Deck.Discard(Card);
        }

        return damage;
    }
}
