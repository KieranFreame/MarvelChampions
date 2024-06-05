using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Nakia Bahadir", menuName = "MarvelChampions/Card Effects/Ms Marvel/Nakia Bahadir")]
public class NakiaBahadir : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Kamala Khan")
            return false;

        if (_card.Exhausted) return false;

        return true;
    }

    public override Task Activate()
    {
        _card.Exhaust();

        foreach (PlayerCard card in _owner.Hand.cards)
        {
            card.CardCost--;
        }

        DrawCardSystem.OnCardDrawn += OnCardDrawn;
        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;
        TurnManager.OnEndPlayerPhase += EndOfPhase;
        
        return Task.CompletedTask;
    }

    private void OnCardDrawn(PlayerCard arg0)
    {
        arg0.CardCost--;
    }

    private void OnCardPlayed(PlayerCard arg0)
    {
        arg0.CardCost++;

        foreach (PlayerCard card in _owner.Hand.cards)
        {
            card.CardCost++;
        }

        DrawCardSystem.OnCardDrawn -= OnCardDrawn;
        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }

    private void EndOfPhase()
    {
        foreach (PlayerCard card in _owner.Hand.cards)
        {
            card.CardCost++;
        }

        DrawCardSystem.OnCardDrawn -= OnCardDrawn;
        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
        TurnManager.OnEndPlayerPhase -= EndOfPhase;
    }
}
