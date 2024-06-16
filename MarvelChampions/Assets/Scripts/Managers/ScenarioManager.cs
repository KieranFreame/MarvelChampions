using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        SOTPResolved = false;

        DontDestroyOnLoad(this);
    }

    public VillainData villain;

    public List<CardData> Obligations { get; set; } = new();

    public List<CardData> RemovedFromGame { get; private set; } = new();

    public Difficulty Difficulty;
    public bool SOTPResolved { get; set; } = false;

    public static ObservableCollection<SchemeCard> sideSchemes = new();

    [SerializeField] private HashSet<string> encounterSets;
    public HashSet<string> EncounterSets {
        get
        {
            encounterSets ??= new();
            return encounterSets;
        }
        set => encounterSets = value; 
    }

    public Deck EncounterDeck;

    public List<SchemeCardData> MainSchemeDeck = new();
    public MainSchemeCard MainScheme { get; private set; }
    public Villain ActiveVillain { get; set; }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameBoardScene")
        {
            ActiveVillain = FindObjectOfType<Villain>();
            ActiveVillain.LoadData(villain);

            GenerateDeck();
        }
    }

    public async void GenerateDeck()
    {
        List<CardData> mainschemeCards = new(EncounterDeck.deck.Where(x => x.cardType is CardType.MainScheme));
        EncounterDeck.deck.RemoveAll(x => x.cardType is CardType.MainScheme);

        for (int i = 1; i <= mainschemeCards.Count; i++)
        {
            MainSchemeDeck.Add(mainschemeCards.Find(x => x.cardID.Contains(i.ToString("000"))) as SchemeCardData);
        }

        MainScheme = CreateCardFactory.Instance.CreateCard(MainSchemeDeck[0], GameObject.Find("MainScheme").transform) as MainSchemeCard;

        if (MainSchemeDeck[0].effect != null)
            await MainSchemeDeck[0].effect.WhenRevealed(ActiveVillain, MainScheme, null);

        MainScheme.Threat.WhenCompleted += NextMainScheme;

        foreach (string s in encounterSets)
        {
            EncounterDeck.AddToDeck(TextReader.PopulateDeck("Modulars/" + string.Concat(s.Where(c => !char.IsWhiteSpace(c))) + ".txt"));
        }

        EncounterDeck.AddToDeck(Obligations);

        /*EncounterDeck.AddToDeck(TextReader.PopulateDeck("Standard.txt"));

        if (Difficulty == Difficulty.Expert) 
            EncounterDeck.AddToDeck(TextReader.PopulateDeck("Expert.txt"));*/
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

        if (MainScheme.Effect != null)
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
