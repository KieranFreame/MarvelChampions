using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayUI : MonoBehaviour
{
    private PlayerCardData cardData;

    public PlayerCardData CardData 
    { 
        get => cardData;
        set
        {
            cardData = value;
            LoadData();
        } 
    }

    public Image CardBase;
    public Image CardArt;
    public TMP_Text CardName;
    public TMP_Text CardCost;
    public TMP_Text CardEffect;

    //Stats
    public TMP_Text AllyAttack;
    public TMP_Text AllyThwart;
    public TMP_Text AllyHealth;

    private void LoadData()
    {
        CardName.text = cardData.cardName;
        CardArt.sprite = cardData.cardArt;

        CardEffect.text = cardData.cardDesc;
        CardBase.color = cardData.cardAspect switch
        {
            Aspect.Aggression => Color.red,
            Aspect.Justice => Color.yellow,
            Aspect.Leadership => Color.blue,
            Aspect.Protection => Color.green,
            _ => Color.grey //Basic
        };

        if (cardData.cardType != CardType.Resource)
            CardCost.text = cardData.cardCost.ToString();

        if (cardData.cardType == CardType.Ally)
        {
            AllyAttack.text = (cardData as AllyCardData).BaseATK.ToString();
            AllyThwart.text = (cardData as AllyCardData).BaseTHW.ToString();
            AllyHealth.text = (cardData as AllyCardData).BaseHP.ToString();
        }
    }
}
