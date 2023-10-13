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

        SOTPResolved.Clear();
    }

    //temp
    public VillainData villain;
    public List<CardData> RemovedFromGame { get; private set; } = new();

    public Difficulty Difficulty;
    public List<Player> SOTPResolved { get; set; } = new();

    public static ObservableCollection<SchemeCard> sideSchemes = new();

    public List<string> EncounterSets = new();

    public Deck EncounterDeck;

    public List<SchemeCardData> MainSchemeDeck = new();
    public MainSchemeCard MainScheme { get; private set; }
    public Villain ActiveVillain { get; set; }

    private void Start()
    {
        ActiveVillain = GameObject.Find("VillainIdentityProfile").GetComponent<Villain>();
        ActiveVillain.LoadData(villain);
        GenerateDeck(villain.deckPath);
    }

    public async void GenerateDeck(string deckPath)
    {
        EncounterDeck = new(deckPath + ".txt");

        List<CardData> mainschemeCards = new(EncounterDeck.deck.Where(x => x.cardType is CardType.MainScheme));

        for (int i = EncounterDeck.deck.Count-1; i >= 0; i--)
        {
            if (EncounterDeck.deck[i].cardType == CardType.MainScheme)
                EncounterDeck.deck.RemoveAt(i);
        }

        for (int i = 1; i <= mainschemeCards.Count; i++)
        {
            MainSchemeDeck.Add(mainschemeCards.Find(x => x.cardID.Contains(i.ToString("000"))) as SchemeCardData);
        }

        MainScheme = CreateCardFactory.Instance.CreateCard(MainSchemeDeck[0], GameObject.Find("MainScheme").transform) as MainSchemeCard;

        if (MainSchemeDeck[0].effect != null)
            await MainSchemeDeck[0].effect.WhenRevealed(ActiveVillain, MainScheme, null);

        MainScheme.Threat.WhenCompleted += NextMainScheme;

        foreach (string s in EncounterSets)
        {
            EncounterDeck.AddToDeck(TextReader.PopulateDeck(s + ".txt"));
        }

        switch (Difficulty)
        {
            case Difficulty.Standard:
                EncounterDeck.AddToDeck(TextReader.PopulateDeck("Standard.txt"));
                break;
            case Difficulty.Expert:
                EncounterDeck.AddToDeck(TextReader.PopulateDeck("Standard.txt"));
                EncounterDeck.AddToDeck(TextReader.PopulateDeck("Expert.txt"));
                break;
        }
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

    private async void NextMainScheme()
    {
        MainScheme.Threat.WhenCompleted -= NextMainScheme;
        await MainScheme.Effect.WhenCompleted();
        MainSchemeDeck.RemoveAt(0);

        if (MainSchemeDeck.Count == 0)
        {
            Debug.Log("You Lose");
            Time.timeScale = 0;
        }
        else
        {
            MainScheme = CreateCardFactory.Instance.CreateCard(MainSchemeDeck[0], GameObject.Find("MainScheme").transform) as MainSchemeCard;

            if (MainSchemeDeck[0].effect != null)
                await MainSchemeDeck[0].effect.WhenRevealed(ActiveVillain, MainScheme, null);

            MainScheme.Threat.WhenCompleted += NextMainScheme;
        }
    }

    public void Surge(Player p)
    {
        Debug.Log("Surging");

        p.EncounterCards.AddCard(EncounterDeck.DealCard());
    }

    public bool ThreatPresent()
    {
        return (sideSchemes.Where(x => x.Threat.CurrentThreat > 0).Count() > 0 || MainScheme.Threat.CurrentThreat > 0);
    }
}
