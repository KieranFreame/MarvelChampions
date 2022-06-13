using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    public static CardFactory instance;

    [SerializeField]
    List<GameObject> prefabs = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public GameObject CreateCard(Card card)
    {
        switch (card.data.cardType)
        {
            case CardType.Ally:
                return prefabs[0];
            case CardType.Upgrade:
                return prefabs[1];
            case CardType.Support:
                return prefabs[2];
            case CardType.Event:
                return prefabs[3];
            case CardType.Resource:
                return prefabs[4];
            default:
                return null;
        }
    }
}
