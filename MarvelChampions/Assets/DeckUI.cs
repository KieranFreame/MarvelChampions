using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    private Player owner;
    [SerializeField] private TMP_Text deckCountText;
    public Transform discardPileTransform { get; private set; }
    private ICard topCard;

    private void Awake()
    {
        owner = transform.parent.GetComponentInChildren<Player>();
        discardPileTransform = transform.Find("DiscardPile");
    }

    private void Start()
    {
        deckCountText.text = owner.Deck.deck.Count.ToString();
        owner.Deck.DeckChanged += DeckCountChanged;
    }

    private void DeckCountChanged(CardData data)
    {
        deckCountText.text = owner.Deck.deck.Count.ToString();
    }

    private void OnMouseDown()
    {
        Debug.Log(name);
    }

    public void SetDiscardPile(ICard card)
    {
        if (topCard != null)
            Destroy((topCard as MonoBehaviour).gameObject);

        topCard = card;
        (topCard as IExhaust).Ready();
        (topCard as MonoBehaviour).transform.SetParent(discardPileTransform, false);
        (topCard as MonoBehaviour).transform.SetAsFirstSibling();
        (topCard as MonoBehaviour).transform.localPosition = Vector3.zero;
        (topCard as MonoBehaviour).transform.localScale = Vector3.one;
    }
}
