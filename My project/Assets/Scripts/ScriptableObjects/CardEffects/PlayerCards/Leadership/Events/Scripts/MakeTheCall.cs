using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Make the Call", menuName = "MarvelChampions/Card Effects/Leadership/Make the Call")]
public class MakeTheCall : PlayerCardEffect
{
    List<CardData> allies = new();

    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
    }

    public override bool CanBePlayed()
    {
        allies.Clear();

        foreach (CardData a in _owner.Deck.discardPile.FindAll(x => x is AllyCardData))
            allies.Add(a);

        allies.RemoveAll(x => (x as PlayerCardData).cardCost > _owner.ResourcesAvailable());

        return allies.Count > 0;
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        List<PlayerCard> cards = new();
        foreach (ICard c in CardViewerUI.inst.EnablePanel(allies))
            cards.Add(c as PlayerCard);

        var ally = await TargetSystem.instance.SelectTarget(cards);

        await PlayCardSystem.instance.InitiatePlayCard(new(player, player.Hand.cards, ally));
    }
}
