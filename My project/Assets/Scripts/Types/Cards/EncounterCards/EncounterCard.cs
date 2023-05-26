using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EncounterCard : Card
{
    public int BoostIcons { get; set; }

    public event UnityAction OnBoost;

    protected virtual void WhenDefeated()
    {
        Owner.GetComponent<Villain>().EncounterDeck?.Discard(this);
    }
    public void OnRevealCard()
    {
        Effect?.OnEnterPlay(Owner.GetComponent<Villain>(), this);
    }
    public void OnBoostCard() => OnBoost?.Invoke();
    public override void LoadCardData(CardData data, GameObject owner)
    {
        //EncounterCard
        BoostIcons = (data as EncounterCardData).boostIcons;

        base.LoadCardData(data, owner);
    }
}
