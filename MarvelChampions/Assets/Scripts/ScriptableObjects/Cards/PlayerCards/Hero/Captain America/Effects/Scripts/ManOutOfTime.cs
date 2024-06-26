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

    public override async Task Resolve()
    {
        rogers = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Steve Rogers");

        if (rogers.Identity.ActiveIdentity is not AlterEgo)
        {
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
            {
                rogers.Identity.FlipToAlterEgo();
            }
        }

        if (rogers.Identity.ActiveIdentity is AlterEgo && !rogers.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            if (await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Steve Rogers. Remove this from the game", "Discard hald of the cards in your hand (rounded down)." }) == 1)
            {
                rogers.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        List<PlayerCard> discards = await TargetSystem.instance.SelectTargets(rogers.Hand.cards.ToList(), (int)Math.Floor(rogers.Hand.cards.Count / 2.0), default);

        foreach (var pCard in discards)
        {
            rogers.Hand.Discard(pCard);
        }
    }
}
