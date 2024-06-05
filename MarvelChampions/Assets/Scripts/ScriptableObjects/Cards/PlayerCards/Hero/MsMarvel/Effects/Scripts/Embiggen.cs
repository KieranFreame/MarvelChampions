using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Embiggen", menuName = "MarvelChampions/Card Effects/Ms Marvel/Embiggen")]
public class Embiggen : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Ms Marvel")
            return false;

        if (_card.Exhausted)
            return false;

        return true;
    }

    public override Task Activate()
    {
        _card.Exhaust();

        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;

        return Task.CompletedTask;
    }

    private void OnCardPlayed(PlayerCard card)
    {
        //if it's not an Attack event
        if (card.CardType != CardType.Event) return;
        if (!card.CardTraits.Contains("Attack")) return;

        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        DamageSystem.Instance.Modifiers.Add(IncreaseValue);
    }

    private Task<DamageAction> IncreaseValue(DamageAction action)
    {
        action.Value += 2;
        DamageSystem.Instance.Modifiers.Remove(IncreaseValue);
        return Task.FromResult(action);
    }
}
