using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

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
    private readonly List<CardData> _boostCards = new();

    //public static event UnityAction<int> OnBoostCardsResolved;

    public void DealBoostCards()
    {
        for (int i = 0; i < BoostCardCount; i++)
            _boostCards.Add(ScenarioManager.inst.EncounterDeck.DealCard());
        

        BoostCardCount = 1;
    }

    public async Task<int> FlipCard()
    {
        int value = 0; 

        foreach (CardData boost in _boostCards)
        {
            EncounterCard inst = RevealCardSystem.instance.CreateEncounterCard(boost as EncounterCardData, true).GetComponent<EncounterCard>();
            inst.Flip();
            Debug.Log($"{inst.CardName} is boosting Rhino's Activation by +{inst.BoostIcons}");
            value += inst.BoostIcons;
            await Task.Delay(2000);
            ScenarioManager.inst.EncounterDeck.Discard(inst);
        }

        //OnBoostCardsResolved?.Invoke(value);
        _boostCards.Clear();
        return value;
    }

    public bool IsBoost(CardData card)
    {
        return _boostCards.Contains(card);
    }
}