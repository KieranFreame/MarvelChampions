using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardPoolPanel : MonoBehaviour
{
    [SerializeField] TMP_Text cardCount;
    public PlayerCardData card { get; set; }
    public int count { get; set; } = 0;

    public void Increment()
    {
        count++;
        cardCount.text = "x" + count;
    }

    public void Decrement()
    {
        count--;
        cardCount.text = "x" + count;
    }

    //Add Button Function
    public void AddCardToDeck()
    {
        if (card != null)
        {
            DeckPreviewPanel.playerDeck.Add(card);

            if (DeckPreviewPanel.cardTabs[card].GetComponent<CardPoolPanel>().count == card.maxCopies)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No Card Data assigned to this tab");
        }
    }

    //Remove Button Function
    public void RemoveCardFromDeck()
    {
        if (card != null)
        {
            CardSearchPanel.ContentTransform.GetComponentsInChildren<CardPoolPanel>(true).FirstOrDefault(c => c.card == card).gameObject.SetActive(true);
            DeckPreviewPanel.playerDeck.Remove(card);
        }
        else
        {
            Debug.Log("No Card Data assigned to this tab");
        }
    }
}
