using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderFactory : MonoBehaviour
{
    public static LoaderFactory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public AbilityLoader CreateLoader(Card card)
    {
        switch (card.data.abilityLoader)
        {
            case "ResourceLoader":
                return new ResourceLoader(card);
            case "OnEnterPlayLoader":
                return new OnEnterPlayLoader(card);
            default:
                return new AbilityLoader(card);
        }
    }
}
