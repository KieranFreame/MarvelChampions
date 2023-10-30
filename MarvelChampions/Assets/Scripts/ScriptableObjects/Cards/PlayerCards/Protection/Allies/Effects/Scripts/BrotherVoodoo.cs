using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Brother Voodoo", menuName = "MarvelChampions/Card Effects/Protection/Allies/Brother Voodoo")]
public class BrotherVoodoo : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        List<PlayerCardData> events = _owner.Deck.GetTop(5).Where(x => x.cardType == CardType.Event).Cast<PlayerCardData>().ToList();

        if (events.Count > 0)
        {
            List<PlayerCard> cards = CardViewerUI.inst.EnablePanel(events.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();
            PlayerCard card = await TargetSystem.instance.SelectTarget(cards);

            card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
            _owner.Hand.AddToHand(card);
            CardViewerUI.inst.DisablePanel();
            _owner.Deck.Shuffle();
            return;
        }

        Debug.Log("No events in top five cards of deck");
    }
}
