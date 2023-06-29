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
        player = card.Owner;
        _play = transform.Find("Play").gameObject;
        _activate = transform.Find("ActivateAbility");
    }

    protected virtual void OnEnable()
    {
        if (UIManager.MakingSelection) return;

        _play.SetActive(card.CurrZone == Zone.Hand && card.Effect.CanBePlayed());

        if (_activate != null && card.Data.cardType != CardType.Event && card.InPlay)
        {
            _activate.gameObject.SetActive(card.Effect.CanActivate());
        }
    }

    public async void Play()
    {
        var action = new PlayCardAction(player, player.Hand.cards, card);
        gameObject.SetActive(false);
        await PlayCardSystem.instance.InitiatePlayCard(action);
    }

    public async void Activate()
    {
        gameObject.SetActive(false);
        await card.Effect.Activate();
    }
}
