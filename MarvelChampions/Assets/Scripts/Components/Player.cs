using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ICharacter, IExhaust
{
    #region Fields
    public string Name { get => Identity.ActiveIdentity.Name; }
    public Identity Identity { get; private set; }
    public CharacterStats CharStats { get; set; }
    public Deck Deck;
    public Hand Hand = new();
    private List<IAttachment> attachments = new();
    public ObservableCollection<IAttachment> Attachments { get => CardsInPlay.Attachments; set => attachments = new(); }
    public PlayerCards CardsInPlay { get; private set; } = new();
    public PlayerEncounterCards EncounterCards { get; private set; }
    public bool Exhausted { get => Identity.Exhausted; set => Identity.Exhausted = value; }
    public bool CanAttack { get; set; } = false;
    public bool CanThwart { get; set; } = false;

    #endregion

    public delegate int GetResources();
    public List<GetResources> resourceGenerators = new();

    private void Awake()
    {
        EncounterCards = new(GameObject.Find("EncounterCards").transform);
    }

    public void LoadData(HeroData hData, AlterEgoData aData, List<CardData> deck)
    {
        Deck = new(deck);

        Identity = new Identity(this, hData, aData);
        CharStats = new(this, hData, aData);

        Identity.Hero.Effect.LoadEffect(this);
        Identity.AlterEgo.Effect.LoadEffect(this);

        transform.parent.Find("AlterEgoInfo").GetComponent<AlterEgoUI>().LoadUI(this);
        transform.parent.Find("HeroInfo").GetComponent<HeroUI>().LoadUI(this);
    }

    private void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += DrawToHandSize;
    }
    private void OnDisable()
    {
        TurnManager.OnEndPlayerPhase -= DrawToHandSize;
    }
    private async void Start()
    {
        List<CardData> obligations = Deck.deck.Where(x => x is EncounterCardData).Cast<CardData>().ToList();

        foreach (CardData c in obligations)
        {
            Deck.deck.Remove(c);
            ScenarioManager.inst.EncounterDeck.AddToDeck(c);
        }

        for (int i = Deck.deck.Count -1; i >= 0; i--)
        {
            if (Deck.deck[i] == null)
            {
                Deck.deck.RemoveAt(i);
            }
        }

        DrawCardSystem.Instance.DrawCards(new DrawCardsAction(Identity.ActiveIdentity.HandSize, this));

        await Identity.ActiveEffect.Setup();
    }
    public void WhenDefeated() { }
    private void DrawToHandSize()
    {
        while (Hand.cards.Count < Identity.ActiveIdentity.HandSize)
        {
            DrawCardSystem.Instance.DrawCards(new DrawCardsAction(1));
        }
    }
    public void Recover() => CharStats.InitiateRecover();
    public void Ready()=> Identity.Ready();
    public void Exhaust()=>Identity.Exhaust();
    public int ResourcesAvailable(PlayerCard cardToPlay = null)
    {
        int resourceCount = 0;

        List<PlayerCard> hand = Hand.cards.Where(x => x != cardToPlay).ToList();

        foreach (PlayerCard card in hand)
        {
            if (card is ResourceCard)
                resourceCount += (card as ResourceCard).ResourceCount(cardToPlay);
            else
                resourceCount++;
        }

        foreach (PlayerCard c in CardsInPlay.Permanents)
            if (c.Effect is IGenerate)
                resourceCount++;

        if (Identity.ActiveEffect is IGenerate)
            resourceCount++;

        return resourceCount;
    }
    public bool HaveResource(Resource resource = Resource.Any, int amount = 1)
    {
        int count = 0;

        foreach (PlayerCard c in Hand.cards)
        {
            if (c.Resources.Contains(resource) || c.Resources.Contains(Resource.Wild))
            {
                count++;
            }
        }

        foreach (var generator in resourceGenerators)
            count += generator();

        return count >= amount;
    }
}
