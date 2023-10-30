using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Man Out of Time", menuName = "MarvelChampions/Card Effects/Captain America/Man Out of Time")]
public class ManOutOfTime : EncounterCardEffect
{
    Player rogers;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        rogers = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Steve Rogers");

        if (rogers.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?");

            if (decision)
            {
                rogers.Identity.FlipToAlterEgo();
            }
        }

        if (rogers.Identity.ActiveIdentity is AlterEgo && !rogers.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Steve Rogers. Remove this from the game", "Discard hald of the cards in your hand (rounded down)." });

            if (decision == 1)
            {
                rogers.Exhaust();
                ScenarioManager.inst.RemoveFromGame(Card.Data);
                Destroy(Card.gameObject);
                return;
            }
        }

        int amountToDiscard;

        if (rogers.Hand.cards.Count % 2 == 0) //even
        {
            amountToDiscard = rogers.Hand.cards.Count / 2;
        }
        else //odd
        {
            amountToDiscard = (int)Math.Floor((double)(rogers.Hand.cards.Count / 2));
        }

        List<PlayerCard> discards = await TargetSystem.instance.SelectTargets(rogers.Hand.cards.ToList(), amountToDiscard, default);

        foreach (var pCard in discards)
        {
            rogers.Hand.Remove(pCard);
            rogers.Deck.Discard(pCard);
        }
    }
}
