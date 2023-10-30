using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mystical Studies", menuName = "MarvelChampions/Card Effects/Doctor Strange/Mystical Studies")]
public class MysticalStudies : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            if (_owner.Identity.IdentityName != "Stephen Strange")
                return false;

            //Will never happen, but still
            if (!_owner.Deck.deck.Any(x => (x as PlayerCardData).cardAspect == Aspect.Hero) && !_owner.Deck.discardPile.Any(x => (x as PlayerCardData).cardAspect == Aspect.Hero))
                return false;

            return true;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        List<PlayerCardData> cards = _owner.Deck.deck.Where(x => (x as PlayerCardData).cardAspect == Aspect.Hero) as List<PlayerCardData>;
        cards.AddRange(_owner.Deck.discardPile.Where(x => (x as PlayerCardData).cardAspect == Aspect.Hero) as List<PlayerCardData>);

        List<PlayerCard> strangeCards = CardViewerUI.inst.EnablePanel(cards.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();
        PlayerCard search = await TargetSystem.instance.SelectTarget(strangeCards);

        _owner.Deck.Search(search.CardName);

        search.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        _owner.Hand.AddToHand(search);

        CardViewerUI.inst.DisablePanel();
    }
}
