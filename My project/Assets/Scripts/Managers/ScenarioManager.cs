using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);

        scenario.difficulty = Difficulty.Standard;
        SOTPResolved.Clear();
;   }

    public string pathName;
    public List<CardData> RemovedFromGame { get; private set; } = new();

    public List<Player> SOTPResolved { get; set; } = new();

    public Scenario scenario = new();

    public static ObservableCollection<SchemeCard> sideSchemes = new();

    public Deck EncounterDeck;

    public List<SchemeCardData> MainSchemeDeck = new();

    public void GenerateDeck()
    {
        EncounterDeck = new(pathName);

        List<CardData> mainschemeCards = new(EncounterDeck.deck.FindAll(x => x.cardType is CardType.MainScheme));
        EncounterDeck.deck.RemoveAll(x => x.cardType is CardType.MainScheme);

        for (int i = 1; i <= mainschemeCards.Count; i++)
        {
            MainSchemeDeck.Add(mainschemeCards.Find(x => x.cardID.Contains(i.ToString("000"))) as SchemeCardData);
        }

        GameObject mainScheme = Instantiate(PrefabFactory.Instance.CreateEncounterCard(MainSchemeDeck[0]), GameObject.Find("MainScheme").transform, false);
        mainScheme.name = MainSchemeDeck[0].cardName;
        mainScheme.GetComponent<MainSchemeCard>().LoadCardData(MainSchemeDeck[0], FindObjectOfType<Villain>());
    }

    public void RemoveFromGame(CardData card)
    {
        if (EncounterDeck.Contains(card))
        {
            EncounterDeck.deck.Remove(card);
            EncounterDeck.limbo.Remove(card);
            EncounterDeck.discardPile.Remove(card);
        }
        else
        {
            foreach (Player p in TurnManager.Players)
            {
                if (p.Deck.Contains(card))
                {
                    p.Deck.deck.Remove(card);
                    p.Deck.limbo.Remove(card);
                    p.Deck.discardPile.Remove(card);
                }
            }
        }

        RemovedFromGame.Add(card);
    }
}
