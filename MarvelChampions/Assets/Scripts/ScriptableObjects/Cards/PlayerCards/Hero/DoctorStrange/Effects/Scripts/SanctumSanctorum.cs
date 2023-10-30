using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sanctum Sanctorum", menuName = "MarvelChampions/Card Effects/Doctor Strange/Sanctum Sanctorum")]
public class SanctumSanctorum : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Stephen Strange")
            return false;

        if (Card.Exhausted)
            return false;

        if (!_owner.Deck.discardPile.Any(x => x.cardTraits.Contains("Spell")))
            return false;

        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        List<PlayerCardData> cards = _owner.Deck.discardPile.Where(x => x.cardTraits.Contains("Spell")) as List<PlayerCardData>;

        List<PlayerCard> spells = CardViewerUI.inst.EnablePanel(cards.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();
        PlayerCard search = await TargetSystem.instance.SelectTarget(spells);

        var shuffle = search.Data;
        CardViewerUI.inst.DisablePanel();

        _owner.Deck.discardPile.Remove(shuffle);
        _owner.Deck.AddToDeck(shuffle);

        DrawCardSystem.Instance.DrawCards(new(1, _owner));
    }
}
