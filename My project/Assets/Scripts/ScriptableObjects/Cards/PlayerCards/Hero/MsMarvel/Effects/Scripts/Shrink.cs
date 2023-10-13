using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shrink", menuName = "MarvelChampions/Card Effects/Ms Marvel/Shrink")]
public class Shrink : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Ms Marvel")
            return false;

        if (Card.Exhausted)
            return false;

        return true;
    }

    public override Task Activate()
    {
        Card.Exhaust();

        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;

        return Task.CompletedTask;
    }

    private void OnCardPlayed(PlayerCard card)
    {
        //if it's not a Thwart event
        if (card.CardType != CardType.Event) return;
        if (!card.CardTraits.Contains("Thwart")) return;

        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        ThwartSystem.Instance.Modifiers.Add(IncreaseValue);
    }

    private Task<ThwartAction> IncreaseValue(ThwartAction action)
    {
        action.Value += 2;
        ThwartSystem.Instance.Modifiers.Remove(IncreaseValue);
        return Task.FromResult(action);
    }
}
