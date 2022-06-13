using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardSystem : MonoBehaviour
{
    #region Singleton Pattern
    public static DrawCardSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    #region Fields
    public int amount { get; set; }
    public Player player { get; set; }
    #endregion

    public void DrawCards (DrawCardsAction action)
    {
        this.amount = action.value;
        
        if (action.drawer == null)
            player = TurnManager.instance.currPlayer;
        else
            player = action.drawer;

        var targetParent = player.transform.GetComponent<PlayerTransforms>().handTransform;

        for (int i = 0; i < amount; i++)
        {
            var inst = Instantiate(CardFactory.instance.CreateCard(player.deck.deck[0]), targetParent);
            inst.GetComponent<CardUI>().card = player.deck.deck[0];

            if (inst.GetComponent<CardUI>().card.abilityLoader.actions.Count > 0)
                SubjectManager.instance.AttachObserver(inst.GetComponent<CardUI>().card.abilityLoader.observer);
        }

        player.deck.Deal(amount);
    }

    public void DrawCardsTest()
    {
        DrawCardsAction test = new DrawCardsAction(1);
        test.Execute();
    }
}
