using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActions : MonoBehaviour
{
    private void OnEnable()
    {
        var cardUI = transform.parent.transform.GetComponent<CardUI>();

        transform.Find("Play").gameObject.SetActive(true);

        if (!GameObject.Find("PlayerUI").GetComponent<Player>().hand.Contains(cardUI.card))
            transform.Find("Play").gameObject.SetActive(false);
    }

    public void Play()
    {
        var player = GameObject.Find("PlayerUI").GetComponent<Player>();
        var card = transform.parent.GetComponent<CardUI>().card;

        var action = new PlayCardAction(player, player.hand.cards, card);
        action.Execute();
        gameObject.SetActive(false);
    }
}
