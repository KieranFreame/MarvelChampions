using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Shuri", menuName = "MarvelChampions/Card Effects/Black Panther/Shuri")]
public class Shuri : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        List<PlayerCardData> upgrades = _owner.Deck.deck.Where(x => x.cardType is CardType.Upgrade).Cast<PlayerCardData>().ToList();

        if (upgrades.Count == 0)
        {
            Debug.Log("No Upgrades in Deck");
            return;
        }

        List<PlayerCard> cards = CardViewerUI.inst.EnablePanel(upgrades.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();

        PlayerCard cardToAdd = await TargetSystem.instance.SelectTarget(cards);

        _owner.Deck.deck.Remove(cardToAdd.Data);
        _owner.Deck.limbo.Add(cardToAdd.Data);

        cardToAdd.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
        _owner.Hand.AddToHand(cardToAdd);

        _owner.Deck.Shuffle();
    }
}
