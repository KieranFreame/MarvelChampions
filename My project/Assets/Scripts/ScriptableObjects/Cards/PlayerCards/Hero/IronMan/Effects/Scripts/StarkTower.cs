using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Stark Tower", menuName = "MarvelChampions/Card Effects/Iron Man/Stark Tower")]
public class StarkTower : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.ActiveIdentity is not AlterEgo)
            return false;

        if (Card.Exhausted)
            return false;

        if (!_owner.Deck.discardPile.Any(x => x.cardTraits.Contains("Tech") && x.cardType == CardType.Upgrade))
            return false;

        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();
        PlayerCardData card = _owner.Deck.discardPile.Last(x => x.cardTraits.Contains("Tech") && x.cardType == CardType.Upgrade) as PlayerCardData;

        _owner.Deck.discardPile.Remove(card);
        _owner.Deck.limbo.Add(card);
        

        PlayerCard inst = Instantiate(PrefabFactory.instance.CreatePlayerCard(card), GameObject.Find("PlayerHandTransform").transform).GetComponent<PlayerCard>();
        inst.gameObject.name = card.cardName;
        inst.LoadCardData(card, _owner);
        _owner.Hand.AddToHand(inst);
        

        await Task.Yield();
    }
}
