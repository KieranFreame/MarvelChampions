using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
    public HeroData heroData;
    public AlterEgoData alterEgoData;
    public Identity Identity { get; private set; }
    public CharacterStats CharStats { get; set; }
    public Deck Deck { get; private set; }

    [SerializeField] private List<string> cardIDs = new();

    public Hand Hand = new();
    public PlayerCards CardsInPlay { get; private set; } = new();
    public PlayerEncounterCards EncounterCards { get; private set; } = new();

    #endregion
    private void OnEnable()
    {
        Identity = new Identity(this, a:alterEgoData, h:heroData);
        CharStats = new(Identity, heroData, alterEgoData);

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
        Deck = new Deck();

        foreach (string id in Database.GetCardSetByName(Identity.Hero.Name).cardIDs)
        {
            CardData data = Database.GetCardDataById(id);

            if (data != null) //temp
                Deck.AddToDeck(data);
        }

        foreach (string id in cardIDs)
        {
            CardData data = Database.GetCardDataById(id);

            if (data != null) //temp
                Deck.AddToDeck(data);
        }

        DrawCardSystem.instance.DrawCards(new DrawCardsAction(Identity.ActiveIdentity.BaseHandSize));
    }
    private void DrawToHandSize()
    {
        while (Hand.cards.Count < Identity.ActiveIdentity.BaseHandSize)
        {
            DrawCardSystem.instance.DrawCards(new DrawCardsAction(1));
        }
    }
}
