using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static PlayCardSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    public Subject onEnterPlay { get; set; }

    private void Start()
    {
        onEnterPlay = new Subject(this);
    }

    public void InitiatePlayCard(PlayCardAction action) => StartCoroutine(PlayCard(action));

    private IEnumerator PlayCard(PlayCardAction action)
    {
        yield return StartCoroutine(PayCostSystem.instance.GetTargets(action.cardToPlay.data, action.candidates));
        List<Card> discards = PayCostSystem.instance.discards;

        foreach (Card card in discards)
            action.owner.deck.Discard(card);

        if (action.cardToPlay.data.cardType == CardType.Ally)
        {
            action.owner.activeAllies.Add(action.cardToPlay.data as Ally);
            action.owner.hand.Remove(action.cardToPlay);
            MoveCard(action);
            action.cardToPlay.inPlay = true;
        }
        else if (action.cardToPlay.data.cardType == CardType.Event)
        {
            action.cardToPlay.abilityLoader.TriggerAbility();
            action.owner.deck.Discard(action.cardToPlay);
        }
        /*else
        {
            //move card to upgrades+supports
            //send OnEnterPlay message
        }*/

        yield return new WaitUntil(() => onEnterPlay.Notify() == true);

        if (onEnterPlay.observers.Contains(action.cardToPlay.abilityLoader.observer))
            onEnterPlay.Detach(action.cardToPlay.abilityLoader.observer);

        yield return null;
    }

    private void MoveCard(PlayCardAction action)
    {
        List<CardUI> cards = new List<CardUI>();
        GameObject gameObj = null;
        cards.AddRange(GameObject.FindObjectsOfType<CardUI>());

        foreach (CardUI card in cards)
        {
            if (card.card == action.cardToPlay)
                gameObj = card.gameObject;
        }

        if (action.cardToPlay.data.cardType == CardType.Ally)
        {
            gameObj.transform.SetParent(action.owner.GetComponent<PlayerTransforms>().alliesTransform);
        }
        
    }
}
