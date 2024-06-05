using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckbuildingOnClick : MonoBehaviour, IPointerClickHandler
{
    CardDisplayUI cardDisplayUI;
    private PlayerCardData cardData;

    private void Awake()
    {
        cardDisplayUI = GetComponent<CardDisplayUI>();
        cardData = cardDisplayUI.CardData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cardData == null)
            cardData = cardDisplayUI.CardData;

        int deckSize = DeckPreviewPanel.playerDeck.Count;

        if (deckSize >= 50)
        {
            Debug.Log("Cannot add card: Deck can only have 50 cards maximum");
            return;
        }

        if (cardData.cardAspect != Aspect.Basic)
            if (DeckPreviewPanel.chosenAspect != cardData.cardAspect && DeckPreviewPanel.chosenAspect != Aspect.Campaign)
            {
                Debug.Log("Cannot add card: Hero can only use 1 aspect in their deck");
                return;
            }

        int copies = DeckPreviewPanel.playerDeck.Count(x => x.cardName == cardData.cardName);
        if (copies < cardData.maxCopies)
            DeckPreviewPanel.playerDeck.Add(cardData);
        else
            Debug.Log("Cannot add card: Deck contains maximum number of copies");
    }
}
