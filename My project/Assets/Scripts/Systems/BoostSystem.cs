using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostSystem : MonoBehaviour
{
    public static BoostSystem instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public int BoostCardCount { get; set; } = 1;
    private Villain _villain;
    private readonly List<CardData> _boostCards = new();

    public static event UnityAction<int> OnBoostCardsResolved;

    private void Start()
    {
        _villain = FindObjectOfType<Villain>();
    }

    public void DealBoostCards()
    {
        for (int i = 0; i < BoostCardCount; i++){
            _boostCards.Add(_villain.EncounterDeck.deck[0]);
            _villain.EncounterDeck.Deal();
        }

        BoostCardCount = 1;
    }

    public IEnumerator FlipCard(System.Action<int> callback)
    {
        int value = 0; 

        foreach (CardData boost in _boostCards)
        {
            EncounterCard inst = RevealCardSystem.instance.CreateEncounterCard(boost as EncounterCardData, true).GetComponent<EncounterCard>();
            inst.Flip();
            Debug.Log($"{inst.CardName} is boosting Rhino's Activation by +{inst.BoostIcons}");
            value += inst.BoostIcons;
            yield return new WaitForSeconds(2);
            _villain.EncounterDeck.Discard(inst.GetComponent<Card>());
        }

        OnBoostCardsResolved?.Invoke(value);
        _boostCards.Clear();

        callback(value);
    }

    public bool IsBoost(CardData card)
    {
        return _boostCards.Contains(card);
    }
}