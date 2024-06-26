using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "Family Emergency", menuName = "MarvelChampions/Card Effects/Captain Marvel/Family Emergency")]
public class FamilyEmergency : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var danvers = TurnManager.Players.FirstOrDefault(x => x.Identity.Hero.Name == "Captain Marvel");

        if (danvers.Identity.ActiveIdentity is not AlterEgo)
        {
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
            {
                danvers.Identity.FlipToAlterEgo();
            }
        }

        if (danvers.Identity.ActiveIdentity is AlterEgo && !danvers.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Carol Danvers. Remove Family Emergency from the game", "Become stunned. Surge." });

            if (decision == 1)
            {
                danvers.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        danvers.CharStats.Attacker.Stunned = true;
        ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
    }
}
