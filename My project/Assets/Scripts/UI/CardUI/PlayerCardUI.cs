using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : CardUI
{
    private PlayerCard playerCard;
    public TMP_Text cardName;
    [SerializeField] protected Image cardArt;
    [SerializeField] protected TMP_Text cardCostText;
    [SerializeField] protected Image cardBase;

    //Resource
    [SerializeField] private Transform resourceParent;
    [SerializeField] private Image resourcePrefab;

    protected virtual void OnEnable()
    {
        if (playerCard == null)
        {
            TryGetComponent(out playerCard);
            playerCard.SetupComplete += LoadData;

            if (playerCard is not ResourceCard)
                playerCard.CardCostChanged += CardCostChanged;
        }
    }

    protected virtual void OnDisable()
    {
        if (playerCard is not ResourceCard)
            playerCard.CardCostChanged -= CardCostChanged;
    }

    protected override void LoadData()
    {
        cardName.text = playerCard.CardName;

        if (cardCostText != null)
            cardCostText.text = playerCard.CardCost.ToString();

        cardBase.color = playerCard.CardAspect switch
        {
            Aspect.Leadership => Color.blue,
            Aspect.Aggression => Color.red,
            Aspect.Justice => Color.yellow,
            Aspect.Protection => Color.green,
            Aspect.Basic => Color.grey,
            _ => Color.black,
        };

        
        if (resourceParent != null && resourceParent.childCount > 0)
            for (int i = 0; i < resourceParent.childCount; i++)
                Destroy(resourceParent.GetChild(i).gameObject);

        for (int i = 0; i < playerCard.Resources.Count; i++)
        {
            Instantiate(resourcePrefab, resourceParent);

            switch (playerCard.Resources[i])
            {
                case Resource.Energy:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.yellow;
                    break;
                case Resource.Scientific:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.blue;
                    break;
                case Resource.Physical:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.red;
                    break;
                case Resource.Wild:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.green;
                    break;
            }

        }

        cardArt.sprite = CardArt;

        playerCard.SetupComplete -= LoadData;
    }

    public virtual void LoadData(PlayerCardData data)
    {
        cardName.text = data.cardName;

        if (cardCostText != null)
            cardCostText.text = data.cardCost.ToString();

        cardBase.color = data.cardAspect switch
        {
            Aspect.Leadership => Color.blue,
            Aspect.Aggression => Color.red,
            Aspect.Justice => Color.yellow,
            Aspect.Protection => Color.green,
            Aspect.Basic => Color.grey,
            _ => Color.black,
        };


        if (resourceParent != null && resourceParent.childCount > 0)
            for (int i = 0; i < resourceParent.childCount; i++)
                Destroy(resourceParent.GetChild(i).gameObject);

        for (int i = 0; i < data.cardResources.Count; i++)
        {
            Instantiate(resourcePrefab, resourceParent);

            switch (data.cardResources[i])
            {
                case Resource.Energy:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.yellow;
                    break;
                case Resource.Scientific:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.blue;
                    break;
                case Resource.Physical:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.red;
                    break;
                case Resource.Wild:
                    resourceParent.GetChild(i).GetComponent<Image>().color = Color.green;
                    break;
            }

        }

        cardArt.sprite = data.cardArt;
    }

    private void CardCostChanged(int newCost)
    {
        cardCostText.text = newCost.ToString();
    }
}
