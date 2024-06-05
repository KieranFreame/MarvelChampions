using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiscardPileUI : MonoBehaviour
{
    private Player owner;
    bool discardPileShowing = false;

    [SerializeField] TMP_Text discardCountText;

    private void Awake()
    {
        owner = TurnManager.instance.CurrPlayer;
        owner.Deck.DiscardChanged += UpdateDiscardCount;
    }

    private void UpdateDiscardCount()
    {
        discardCountText.text = owner.Deck.discardPile.Count.ToString();
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
