using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEncounterCards
{
    public List<CardData> EncounterCards = new();
    private bool resolved = false;

    public PlayerEncounterCards()
    {
        RevealEncounterCardSystem.OnEncounterCardRevealed += Resolved;
    }

    public IEnumerator RevealEncounterCards()
    {
        while (EncounterCards.Count > 0)
        {
            GameObject card = (EncounterCards[0] is EncounterCardData) ?
                RevealCardSystem.instance.CreateEncounterCard(EncounterCards[0] as EncounterCardData, true) : PrefabFactory.instance.CreatePlayerCard(EncounterCards[0] as PlayerCardData);

            resolved = false;

            RevealEncounterCardSystem.instance.InitiateRevealCard(card.GetComponent<EncounterCard>());

            while (!resolved)
                yield return null;

            EncounterCards.RemoveAt(0);
        }
    }

    private void Resolved(Card redundant)
    {
        resolved = true;
    }

    public void AddCard(CardData card) => EncounterCards.Add(card);
}
