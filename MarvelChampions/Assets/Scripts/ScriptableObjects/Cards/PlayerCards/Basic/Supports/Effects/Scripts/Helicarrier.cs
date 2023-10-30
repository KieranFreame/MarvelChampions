using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Helicarrier", menuName = "MarvelChampions/Card Effects/Basic/Supports/Helicarrier")]
public class Helicarrier : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return !Card.Exhausted;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        foreach (PlayerCard c in _owner.Hand.cards)
        {
            c.CardCost--;
        }

        DrawCardSystem.OnCardDrawn += OnCardDrawn;
        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;

        await Task.Yield();
    }

    private void OnCardDrawn(PlayerCard card)
    {
        card.CardCost--;
    }

    private void OnCardPlayed(PlayerCard card)
    {
        DrawCardSystem.OnCardDrawn -= OnCardDrawn;
        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;

        card.CardCost = card.BaseCardCost;

        foreach (PlayerCard c in _owner.Hand.cards)
        {
            c.CardCost = c.BaseCardCost;
        }
    }
}
