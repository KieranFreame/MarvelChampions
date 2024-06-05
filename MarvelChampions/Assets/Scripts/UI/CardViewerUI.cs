using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardViewerUI : MonoBehaviour
{
    Transform cardViewerPanel;
    public Transform content;
    public Transform aside;

    public static CardViewerUI inst;

    private List<CardData> _cardsOnDisplay = new();

    private void Awake()
    {
        inst = this;
        cardViewerPanel = transform.GetChild(0);
    }

    public List<ICard> EnablePanel(List<CardData> cardsToDisplay)
    {
        List<ICard> cardsOnDisplay = new();
        _cardsOnDisplay = cardsToDisplay;
        cardViewerPanel.gameObject.SetActive(true);

        foreach (CardData c in cardsToDisplay)
        {
            ICard card = CreateCardFactory.Instance.CreateCard(c, content);
            cardsOnDisplay.Add(card);
        }

        return cardsOnDisplay;
    }

    public void DisablePanel()
    {
        foreach (Transform child in content.transform)
            Destroy(child.gameObject);

        cardViewerPanel.gameObject.SetActive(false);
    }
}
