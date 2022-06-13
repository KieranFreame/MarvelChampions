using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyActions : MonoBehaviour
{
    private void OnEnable()
    {
        var cardUI = transform.parent.transform.GetComponent<CardUI>();
        var player = GameObject.Find("PlayerUI").GetComponent<Player>();

        transform.Find("Attack").gameObject.SetActive(false);
        transform.Find("Thwart").gameObject.SetActive(false);
        transform.Find("ActivateAbility").gameObject.SetActive(false);
        transform.Find("Play").gameObject.SetActive(false);

        if (GameObject.Find("PlayerUI").GetComponent<Player>().hand.Contains(cardUI.card))
        {
            transform.Find("Play").gameObject.SetActive(true);
            return;
        }

        if (!cardUI.card.exhausted || cardUI.GetComponent<Attacker>() != null)
            transform.Find("Attack").gameObject.SetActive(true);
        
        if (!cardUI.card.exhausted || cardUI.GetComponent<Thwarter>() != null)
            transform.Find("Thwart").gameObject.SetActive(true);

        if (!player.hand.Contains(cardUI.card))
            transform.Find("ActivateAbility").gameObject.SetActive(true);
    }

    public void Attack()
    {
        transform.parent.transform.GetComponent<Attacker>().Attack();
        gameObject.SetActive(false);
    }
    public void Thwart()
    {
        transform.parent.transform.GetComponent<Thwarter>().Thwart();
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        transform.parent.transform.GetComponent<EventTrigger>().ActivateAbility();
        gameObject.SetActive(false);
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
