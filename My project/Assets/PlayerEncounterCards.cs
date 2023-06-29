using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEncounterCards
{
    public List<CardData> EncounterCards = new();

    public async Task RevealEncounterCards()
    {
        while (EncounterCards.Count > 0)
        {
            GameObject card = (EncounterCards[0] is EncounterCardData) ?
                RevealCardSystem.instance.CreateEncounterCard(EncounterCards[0] as EncounterCardData, true) : PrefabFactory.instance.CreatePlayerCard(EncounterCards[0] as PlayerCardData);

            await RevealEncounterCardSystem.instance.InitiateRevealCard(card.GetComponent<EncounterCard>());

            EncounterCards.RemoveAt(0);
        }
    }

    public void AddCard(CardData card) => EncounterCards.Add(card);
}
