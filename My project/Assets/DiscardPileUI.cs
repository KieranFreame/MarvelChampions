using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPileUI : MonoBehaviour
{
    private Player owner;
    bool discardPileShowing = false;

    private void Awake()
    {
        owner = transform.parent.transform.parent.transform.parent.GetComponentInChildren<Player>();
    }

    public void OpenDiscardPile()
    {
        if (owner.Deck.discardPile.Count == 0) return;

        discardPileShowing = !discardPileShowing;

        if (discardPileShowing)
        {
            List<PlayerCard> discards = CardViewerUI.inst.EnablePanel(owner.Deck.discardPile).Cast<PlayerCard>().ToList();

            foreach (PlayerCard card in discards)
            {
                card.CurrZone = Zone.Discard;
            }
        }
        else
            CardViewerUI.inst.DisablePanel();
    }
}
