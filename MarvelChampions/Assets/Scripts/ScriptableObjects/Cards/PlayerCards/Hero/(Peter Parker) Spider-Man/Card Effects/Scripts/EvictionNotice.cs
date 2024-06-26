using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Eviction Notice", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Eviction Notice")]
public class EvictionNotice : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var parker = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Peter Parker");

        if (parker.Identity.ActiveIdentity is not AlterEgo)
        {
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
            {
                parker.Identity.FlipToAlterEgo();
            }
        }

        if (parker.Identity.ActiveIdentity is AlterEgo && !parker.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Peter Parker. Remove Eviction Notice from the game", "Discard 1 card from your hand randomly. Surge." });

            if (decision == 1)
            {
                parker.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        PlayerCard discard = parker.Hand.cards[Random.Range(0, parker.Hand.cards.Count)];

        Debug.Log(discard.CardName + " has been discarded");

        parker.Hand.Discard(discard);

        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);

    }
}
