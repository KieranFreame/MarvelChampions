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
    public List<IAttachment> Attachments { get => CardsInPlay.Attachments.ToList(); set => attachments = new(); }
    public PlayerCards CardsInPlay { get; private set; } = new();
    public PlayerEncounterCards EncounterCards { get; private set; } = new();
    public bool Exhausted { get => Identity.Exhausted; set => Identity.Exhausted = value; }

    #endregion
    private void Awake()
    {
        Identity = new Identity(this, a:alterEgoData, h:heroData);
        CharStats = new(this, heroData, alterEgoData);

        Deck = new(path);

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
    private void Start()
    {
        List<CardData> obligations = Deck.deck.FindAll(x => x is EncounterCardData);

        foreach (CardData c in obligations)
        {
            Deck.deck.Remove(c);
            ScenarioManager.inst.EncounterDeck.AddToDeck(c);
        }

        Deck.deck.RemoveAll(x => x == null);

        DrawCardSystem.instance.DrawCards(new DrawCardsAction(Identity.ActiveIdentity.HandSize, this));
    }
    private void DrawToHandSize()
    {
        while (Hand.cards.Count < Identity.ActiveIdentity.HandSize)
        {
            DrawCardSystem.instance.DrawCards(new DrawCardsAction(1));
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
            if (c.Effect is IResourceGenerator)
                resourceCount++;

        if (Identity.ActiveEffect is IResourceGenerator)
            resourceCount++;

        return resourceCount;
    }
}
