using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Bruno Carrelli", menuName = "MarvelChampions/Card Effects/Ms Marvel/Bruno Carrelli")]
public class BrunoCarrelli : PlayerCardEffect
{
    Counters cardsStored;
    List<PlayerCardData> storedCards = new();

    public override Task OnEnterPlay()
    {
        cardsStored = Card.gameObject.AddComponent<Counters>();
        return base.OnEnterPlay();
    }

    public override bool CanActivate()
    {
        if (Card.Exhausted)
            return false;

        if (_owner.Identity.IdentityName == "Kamala Khan") //alterego
        {
            if (storedCards.Count == 0 && _owner.Hand.cards.Count == 0)
                return false;
        }
        else //hero
        {
            if (storedCards.Count == 0)
                return false;
        }

        return true;
    }

    public override async Task Activate()
    {
        if (_owner.Identity.IdentityName == "Kamala Khan") //alterego
        {
            if (storedCards.Count == 0)
            {
                await AlterEgoAction();
                return;
            }

            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "", "" });

            if (decision == 1)
            {
                await AlterEgoAction();
                return;
            }
        }

        await Action();

        return;
    }

    private async Task AlterEgoAction()
    {
        Card.Exhaust();

        PlayerCard card = await TargetSystem.instance.SelectTarget(_owner.Hand.cards.ToList());

        _owner.Hand.Remove(card);
        _owner.Deck.limbo.Remove(card.Data); //don't shuffle back on reset

        storedCards.Add(card.Data);

        Destroy(card.gameObject);

        cardsStored.AddCounters(1);
    }

    private async Task Action()
    {
        Card.Exhaust();

        List<PlayerCard> storage = CardViewerUI.inst.EnablePanel(storedCards.Cast<CardData>().ToList()).Cast<PlayerCard>().ToList();

        CancellationToken token = FinishButton.ToggleFinishButton(true, FinishedSelection);

        int amount = (storedCards.Count < 3) ? storedCards.Count : 3;

        List<PlayerCard> cardsToGet = await TargetSystem.instance.SelectTargets(storage, amount, token);

        FinishButton.ToggleFinishButton(false, FinishedSelection);

        foreach (PlayerCard card in cardsToGet)
        {
            card.PrevZone = card.CurrZone;
            card.CurrZone = Zone.Hand;

            card.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
            _owner.Hand.AddToHand(card);
            card.InPlay = false;

            storedCards.Remove(card.Data);
            cardsStored.RemoveCounters(1);
        }

        CardViewerUI.inst.DisablePanel();
    }

    private void FinishedSelection()
    {
        FinishButton.ToggleFinishButton(false, FinishedSelection);
    }

    public override void OnExitPlay()
    {
        foreach (PlayerCardData card in storedCards)
        {
            _owner.Deck.limbo.Add(card);
            _owner.Deck.Discard(CreateCardFactory.Instance.CreateCard(card, null));
        }
    }
}
