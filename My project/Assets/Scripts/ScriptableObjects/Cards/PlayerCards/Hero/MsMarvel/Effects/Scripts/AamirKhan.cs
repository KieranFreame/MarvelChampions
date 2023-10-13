using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Aamir Khan", menuName = "MarvelChampions/Card Effects/Ms Marvel/Aamir Khan")]
public class AamirKhan : PlayerCardEffect
{
    public override bool CanActivate()
    {
        if (_owner.Identity.IdentityName != "Kamala Khan")
            return false;

        if (_owner.Deck.discardPile.Count == 0)
            return false;

        if (Card.Exhausted) 
            return false; 
        
        return true;
    }

    public override async Task Activate()
    {
        Card.Exhaust();

        List<PlayerCard> discardPile = CardViewerUI.inst.EnablePanel(_owner.Deck.discardPile).Cast<PlayerCard>().ToList();
        PlayerCard choice = await TargetSystem.instance.SelectTarget(discardPile);

        _owner.Deck.discardPile.Remove(choice.Data);
        _owner.Deck.deck.Add(choice.Data);

        CardViewerUI.inst.DisablePanel();

        DrawCardSystem.Instance.DrawCards(new(1, _owner));
    }
}
