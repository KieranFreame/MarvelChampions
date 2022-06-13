using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [SerializeField]
    List<Card> cards = new List<Card>();

    public Card GetPlayerCard(string cardName)
    {
        foreach (Card card in cards)
        {
            if (card.data.cardName == cardName)
                return card;
        }

        return null;
    }
}
