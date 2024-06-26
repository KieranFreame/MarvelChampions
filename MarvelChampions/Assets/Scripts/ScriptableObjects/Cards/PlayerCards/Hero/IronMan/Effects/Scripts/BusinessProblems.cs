using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Business Problems", menuName = "MarvelChampions/Card Effects/Iron Man/Business Problems")]
public class BusinessProblems : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var stark = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Tony Stark");

        if (stark.Identity.ActiveIdentity is not AlterEgo)
        {
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
            {
                stark.Identity.FlipToAlterEgo();
            }
        }

        if (stark.Identity.ActiveIdentity is AlterEgo && !stark.Exhausted)
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Tony Stark. Remove this card from the game", "Exhaust all your upgrades" });

            if (decision == 1)
            {
                stark.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        List<PlayerCard> upgrades = new(stark.CardsInPlay.Permanents.Where(x => x.CardType == CardType.Upgrade));

        foreach (var c in upgrades)
        {
            c.Exhaust();
        }
    }
}
