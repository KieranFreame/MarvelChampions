using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hellcat", menuName = "MarvelChampions/Card Effects/She-Hulk/Hellcat")]
public class Hellcat : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return Card.InPlay;
    }

    public override async Task Activate()
    {
        Card.Ready();

        (Card as AllyCard).CharStats.Health.RecoverHealth(3);

        foreach (IAttachment card in (Card as AllyCard).Attachments)
            _owner.Deck.Discard(card as ICard);

        Card.PrevZone = Card.CurrZone;
        Card.CurrZone = Zone.Hand;

        Card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform , false);
        _owner.Hand.AddToHand(Card);
        Card.InPlay = false;

        await Task.Yield();
    }
}
