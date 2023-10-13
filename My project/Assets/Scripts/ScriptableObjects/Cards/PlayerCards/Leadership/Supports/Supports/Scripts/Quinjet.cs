using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Quinjet", menuName = "MarvelChampions/Card Effects/Leadership/Quinjet")]
public class Quinjet : PlayerCardEffect
{
    List<AllyCard> avengers;
    Counters time;

    public override Task OnEnterPlay()
    {
        time = Card.gameObject.AddComponent<Counters>();

        TurnManager.OnStartPlayerPhase += AddCounter;
        return Task.CompletedTask;
    }

    private void AddCounter() => time.AddCounters(1);

    public override bool CanActivate()
    {
        avengers = _owner.Hand.cards.Where(x => x is AllyCard && x.CardTraits.Contains("Avenger")).Cast<AllyCard>().ToList();

        if (avengers.Count == 0) return false;
        if (avengers.Where(x => x.CardCost <= time.CountersLeft).Count() == 0) return false;

        return true;
    }

    public override async Task Activate()
    {
        AllyCard card = await TargetSystem.instance.SelectTarget(avengers);

        _owner.Hand.Remove(card);

        card.transform.SetParent(GameObject.Find("AlliesTransform").transform);
        _owner.CardsInPlay.Allies.Add(card);

        if (_owner.CardsInPlay.ReachedAllyLimit())
        {
            Debug.Log("Exceeded Ally Limit, Discard 1 Ally from Play");
            await PlayCardSystem.Instance.DiscardAlly();
        }

        card.PrevZone = card.CurrZone;
        card.CurrZone = Zone.Ally;
        card.InPlay = true;

        await card.OnEnterPlay();

        TurnManager.OnStartPlayerPhase -= AddCounter;
        _owner.Deck.Discard(Card);
    }

    public override void OnExitPlay()
    {
        TurnManager.OnStartPlayerPhase -= AddCounter;
    }
}
