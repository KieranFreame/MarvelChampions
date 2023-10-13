using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter, IExhaust
{
    #region Fields
    public string path;
    public HeroData heroData;
    public AlterEgoData alterEgoData;
    public Identity Identity { get; private set; }
    public CharacterStats CharStats { get; set; }
    public Deck Deck;
    public Hand Hand = new();
    private List<IAttachment> attachments = new();
    public ObservableCollection<IAttachment> Attachments { get => CardsInPlay.Attachments; set => attachments = new(); }
    public PlayerCards CardsInPlay { get; private set; } = new();
    public PlayerEncounterCards EncounterCards { get; private set; }
    public bool Exhausted { get => Identity.Exhausted; set => Identity.Exhausted = value; }
    public bool CanAttack { get; set; } = true;
    public bool CanThwart { get; set; } = true;

    #endregion
    private void Awake()
    {
        Deck = new(path);

        EncounterCards = new(GameObject.Find("EncounterCards").transform);
        Identity = new Identity(this, a: alterEgoData, h: heroData);
        CharStats = new(this, heroData, alterEgoData);

        Identity.Hero.Effect.LoadEffect(this);
        Identity.AlterEgo.Effect.LoadEffect(this);

        transform.parent.Find("AlterEgoInfo").GetComponent<AlterEgoUI>().LoadUI(this);
        transform.parent.Find("HeroInfo").GetComponent<HeroUI>().LoadUI(this);
    }
    private void OnEnable()
    {
        TurnManager.OnEndPlayerPhase += DrawToHandSize;
        TurnManager.OnEndPlayerPhase += Identity.EndPlayerPhase;
    }
    private void OnDisable()
    {
        TurnManager.OnEndPlayerPhase -= DrawToHandSize;
        TurnManager.OnEndPlayerPhase -= Identity.EndPlayerPhase;
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
    public bool HaveResource(Resource resource, int amount = 1)
    {
        int count = 0;

        foreach (PlayerCard c in Hand.cards)
        {
            if (c.Resources.Contains(resource))
            {
                count++;
            }
        }

        if (Identity.ActiveEffect is IGenerate)
        {
            IGenerate eff = (IGenerate)Identity.ActiveEffect;

            if (eff.CompareResource(resource))
            {
                count++;
            }
        }

        count += CardsInPlay.HaveResource(resource, amount);

        return count >= amount;
    }
}
