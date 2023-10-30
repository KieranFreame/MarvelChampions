using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCardFactory
{
    private ICard cardToReturn;
    private static CreateCardFactory instance = new();

    public ICard CreateCard(CardData data, Transform parent)
    {
        GameObject card = Object.Instantiate(PrefabFactory.instance.GetPrefab(data), parent);

        card.name = data.cardName;

        if (data is PlayerCardData)
        {
            cardToReturn = card.GetComponent<PlayerCard>();
            (cardToReturn as PlayerCard).LoadCardData(data as PlayerCardData, TurnManager.instance.CurrPlayer);
        }
        else
        {
            if (data is SchemeCardData || data is MinionCardData)
                cardToReturn = card.GetComponentInChildren<EncounterCard>();
            else
                cardToReturn = card.GetComponent<EncounterCard>();


            (cardToReturn as EncounterCard).LoadCardData(data as EncounterCardData, ScenarioManager.inst.ActiveVillain);
        }

        return cardToReturn;
    }

    public static CreateCardFactory Instance
    {
        get => instance;
    }
}
