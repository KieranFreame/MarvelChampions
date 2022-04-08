using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Text cardNameText;
    public Text cardDescText;
    public Text cardTagsText;

    protected Card card;

    protected virtual void Start()
    {
        card = GetComponent<Card>();

        cardNameText.text = card._cd.cardName;
        cardDescText.text = card._cd.cardDescription;

        if (cardTagsText != null)
        {
            for (int i = 0; i < card._cd.cardTags.Count - 1; i++)
            {
                cardTagsText.text += $"{card._cd.cardTags[i]}, ";
            }
            cardTagsText.text += card._cd.cardTags[card._cd.cardTags.Count - 1];
        }  
    }
}
