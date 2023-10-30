using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Steve Rogers", menuName = "MarvelChampions/Identity Effects/Captain America/AlterEgo")]
public class SteveRogers : IdentityEffect
{
    bool allyPlayed;

    public override void LoadEffect(Player _owner)
    {
        owner = _owner;

        owner.CardsInPlay.Allies.CollectionChanged += AllyPlayed;
        TurnManager.OnEndPlayerPhase += Reset;
    }

    private void AllyPlayed(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (allyPlayed || owner.Identity.IdentityName != "Steve Rogers") return;
            else
            {
                (e.NewItems[0] as AllyCard).CardCost++;

                foreach (PlayerCard a in owner.Hand.cards.Where(x => x is AllyCard))
                {
                    a.CardCost++;
                }
            }

            allyPlayed = true;
        }
    }

    private void OnCardDrawn(PlayerCard arg0)
    {
        if (allyPlayed) return;

        if (arg0 is AllyCard && owner.Identity.IdentityName == "Steve Rogers")
        {
            arg0.CardCost--;
        }
    }

    public override void OnFlipUp()
    {
        if (!allyPlayed)
        {
            foreach (AllyCard a in owner.Hand.cards.Where(x => x is AllyCard))
            {
                a.CardCost--;
            }

            DrawCardSystem.OnCardDrawn += OnCardDrawn;
        }
    }

    public override void OnFlipDown()
    {
        if (!allyPlayed)
        {
            foreach (AllyCard a in owner.Hand.cards.Where(x => x is AllyCard))
            {
                a.CardCost++;
            }

            DrawCardSystem.OnCardDrawn -= OnCardDrawn;
        }
    }

    protected override void Reset()
    {
        allyPlayed = false;

        if (owner.Identity.IdentityName == "Steve Rogers")
        {
            foreach (AllyCard a in owner.Hand.cards.Where(x => x is AllyCard))
            {
                if (a.CardCost == a.BaseCardCost)
                    a.CardCost--;
            }
        }
    }

    public override Task Setup()
    {
        PlayerCardData data = owner.Deck.Search("Captain America's Shield") as PlayerCardData;

        if (data == null) //in hand already
            return Task.CompletedTask;

        PlayerCard shield = CreateCardFactory.Instance.CreateCard(data, GameObject.Find("PlayerHandTransform").transform) as PlayerCard;
        owner.Hand.AddToHand(shield);

        allyPlayed = false;

        foreach (AllyCard a in owner.Hand.cards.Where(x => x is AllyCard))
        {
            a.CardCost--;
        }

        DrawCardSystem.OnCardDrawn += OnCardDrawn;

        return Task.CompletedTask;
    }
}
