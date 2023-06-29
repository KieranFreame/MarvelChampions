using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Helicarrier", menuName = "MarvelChampions/Card Effects/Basic/Helicarrier")]
public class Helicarrier : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;

        await Task.Yield();
    }

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
        PlayCardSystem.instance.OnCardPlayed += OnCardPlayed;

        await Task.Yield();
    }

    private void OnCardDrawn(PlayerCard card)
    {
        card.CardCost--;
    }

    private void OnCardPlayed(PlayerCard card)
    {
        DrawCardSystem.OnCardDrawn -= OnCardDrawn;
        PlayCardSystem.instance.OnCardPlayed -= OnCardPlayed;

        card.CardCost = card.BaseCardCost;

        foreach (PlayerCard c in _owner.Hand.cards)
        {
            c.CardCost = c.BaseCardCost;
        }
    }
}
