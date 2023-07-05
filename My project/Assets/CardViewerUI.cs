using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardViewerUI : MonoBehaviour
{
    Transform cardViewerPanel;
    public Transform aside;

    public static CardViewerUI inst;

    private List<CardData> _cardsOnDisplay = new();

    private void Awake()
    {
        inst ??= this;
        if (inst != this) Destroy(this);

        cardViewerPanel = transform.Find("CardViewerPanel");
    }

    public List<ICard> EnablePanel(List<CardData> cardsToDisplay)
    {
        List<ICard> cardsOnDisplay = new();
        _cardsOnDisplay = cardsToDisplay;
        cardViewerPanel.gameObject.SetActive(true);
        GameObject card;

        foreach (CardData c in cardsToDisplay)
        {
            if (c is EncounterCardData)
            {
                card = Instantiate(PrefabFactory.instance.CreateEncounterCard(c as EncounterCardData), cardViewerPanel);
            }
            else
            {
                card = Instantiate(PrefabFactory.instance.CreatePlayerCard(c as PlayerCardData), cardViewerPanel);
                card.GetComponent<PlayerCard>().LoadCardData(c as PlayerCardData, TurnManager.instance.CurrPlayer);
            }

            cardsOnDisplay.Add(card.GetComponent<ICard>());
        }

        TargetSystem.TargetAcquired += DisablePanel;
        return cardsOnDisplay;
    }

    private void DisablePanel(dynamic card)
    {
        TargetSystem.TargetAcquired -= DisablePanel;

        card.transform.SetParent(aside, false);

        foreach (Transform child in cardViewerPanel.transform)
            Destroy(child.gameObject);

        cardViewerPanel.gameObject.SetActive(false);
    }
}
