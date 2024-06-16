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
    [SerializeField] private TMP_Text discardCountText;

    private void Awake()
    {
        owner = transform.parent.GetComponentInChildren<Player>();
    }

    private void Start()
    {
        deckCountText.text = owner.Deck.deck.Count.ToString();
        owner.Deck.DeckChanged += DeckCountChanged;
    }

    private void DeckCountChanged()
    {
        deckCountText.text = owner.Deck.deck.Count.ToString();
        discardCountText.text = owner.Deck.discardPile.Count.ToString();
    }

    private void OnMouseDown()
    {
        Debug.Log(name);
    }
}
