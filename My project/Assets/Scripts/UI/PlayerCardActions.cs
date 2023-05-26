using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardActions : MonoBehaviour
{
    /// <summary>
    /// For toggling UI for Player Cards
    /// </summary>
    protected PlayerCard card;
    protected Player player;

    private GameObject _play;
    private Transform _activate;

    protected virtual void Awake()
    {
        card =  transform.parent.transform.GetComponent<PlayerCard>();
        player = card.Owner.GetComponent<Player>();
        _play = transform.Find("Play").gameObject;
        _activate = transform.Find("ActivateAbility");
    }

    protected virtual void OnEnable()
    {
        if (UIManager.InStateMachine) return;

        _play.SetActive(card.CurrZone == Zone.Hand);

        if (_activate != null && card.CardType != CardType.Event && card.InPlay)
        {
            _activate.gameObject.SetActive(card.Effect.CanActivate());
        }
    }

    public void Play()
    {
        var action = new PlayCardAction(player.gameObject, player.Hand.cards, card);
        PlayCardSystem.instance.InitiatePlayCard(action);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        card.Effect.Activate();
        gameObject.SetActive(false);
    }
}
