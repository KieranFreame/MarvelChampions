using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PlayerEncounterCards
{
    Transform transform;
    public List<CardData> EncounterCards = new();
    TMP_Text ui;

    public PlayerEncounterCards(Transform parent)
    {
        transform = parent;
        ui = GameObject.Find("ECCount").GetComponent<TMP_Text>();
    }

    public async Task RevealEncounterCards()
    {
        while (EncounterCards.Count > 0)
        {
            ICard card = CreateCardFactory.Instance.CreateCard(EncounterCards[0], transform);

            await RevealEncounterCardSystem.Instance.InitiateRevealCard(card as EncounterCard);

            EncounterCards.RemoveAt(0);
            ui.text = EncounterCards.Count.ToString();
        }
    }

    public void AddCard(CardData card)
    {
        EncounterCards.Add(card);
        ui.text = EncounterCards.Count.ToString();
    }
}
