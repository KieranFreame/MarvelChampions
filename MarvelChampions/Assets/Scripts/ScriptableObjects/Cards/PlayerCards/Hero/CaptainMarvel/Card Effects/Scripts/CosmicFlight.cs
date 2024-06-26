using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Cosmic Flight", menuName = "MarvelChampions/Card Effects/Captain Marvel/Cosmic Flight")]
public class CosmicFlight : PlayerCardEffect
{
    bool addedTrait = false;

    public override async Task OnEnterPlay()
    {
        if (!_owner.Identity.IdentityTraits.Contains("Aerial"))
        {
            _owner.Identity.IdentityTraits.AddItem("Aerial");
            addedTrait = true;
        }
            

        _owner.CharStats.Health.Modifiers.Add(OnTakeDamage);

        await Task.Yield();
    }

    public async Task<DamageAction> OnTakeDamage(DamageAction action)
    {
        bool decision = await ConfirmActivateUI.MakeChoice(Card);

        if (decision)
        {
            action.Value -= 3;

            if (addedTrait)
                _owner.Identity.IdentityTraits.RemoveItem("Aerial");

            _owner.CharStats.Health.Modifiers.Remove(OnTakeDamage);
            _owner.Deck.Discard(Card);  
        }

        return action;
    }
}
