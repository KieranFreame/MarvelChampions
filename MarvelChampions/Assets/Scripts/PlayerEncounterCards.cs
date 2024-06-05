using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEncounterCards
{
    private Transform transform;
    public List<CardData> EncounterCards = new();

    public PlayerEncounterCards(Transform parent)
    {
        transform = parent;
    }

    public async Task RevealEncounterCards()
    {
        while (EncounterCards.Count > 0)
        {
            ICard card = CreateCardFactory.Instance.CreateCard(EncounterCards[0], transform);

            await RevealEncounterCardSystem.Instance.InitiateRevealCard(card as EncounterCard);

            EncounterCards.RemoveAt(0);
        }
    }

    public void AddCard(CardData card) => EncounterCards.Add(card);
}
