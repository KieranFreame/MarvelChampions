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
        _card.Ready();

        (_card as AllyCard).CharStats.Health.CurrentHealth += 3;

        foreach (IAttachment card in (_card as AllyCard).Attachments)
            _owner.Deck.Discard(card as ICard);

        _card.PrevZone = Card.CurrZone;
        _card.CurrZone = Zone.Hand;

        _card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform , false);
        _owner.Hand.AddToHand(_card);
        _card.InPlay = false;

        await Task.Yield();
    }
}
