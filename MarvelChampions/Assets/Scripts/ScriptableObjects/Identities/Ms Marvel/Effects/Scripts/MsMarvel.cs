using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ms Marvel", menuName = "MarvelChampions/Identity Effects/Ms Marvel/Hero")]
public class MsMarvel : IdentityEffect
{
    /// <summary>
    /// "Morphogenetics" — Response: After you play an ATTACK, THWART, or DEFENSE event, exhaust Ms. Marvel; return that event to your hand.
    /// </summary>
 
    public override void LoadEffect(Player _owner)
    {
        owner = _owner;
    }

    public override void OnFlipUp()
    {
        PlayCardSystem.Instance.OnCardPlayed += OnCardPlayed;
    }

    private async void OnCardPlayed(PlayerCard arg0)
    {
        if (owner.Exhausted) return;
        if (arg0.CardType != CardType.Event) return;
        if (!arg0.CardTraits.Collection.Any(x => x == "Attack" || x == "Thwart" || x == "Defense")) return;

        bool choice = await ConfirmActivateUI.MakeChoice("Return Card to Hand?");

        if (choice)
        {
            owner.Exhaust();

            arg0.PrevZone = arg0.CurrZone;
            arg0.CurrZone = Zone.Hand;

            arg0.transform.SetParent(GameObject.Find("PlayerHandTransform").transform, false);
            owner.Hand.AddToHand(arg0);
            arg0.InPlay = false;
        }
    }

    public override void OnFlipDown()
    {
        PlayCardSystem.Instance.OnCardPlayed -= OnCardPlayed;
    }
}
