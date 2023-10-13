using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Legal Work", menuName = "MarvelChampions/Card Effects/She-Hulk/Legal Work")]
public class LegalWork : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        Player walters = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Jennifer Walters");

        if (walters.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?");

            if (decision)
            {
                walters.Identity.FlipToAlterEgo();
            }
        }

        if (walters.Identity.ActiveIdentity is AlterEgo && !walters.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Jennifer Walters. Remove Legal Work from the game", "The main scheme gains +1 acceleration" });

            if (decision == 1)
            {
                walters.Exhaust();
                ScenarioManager.inst.RemoveFromGame(Card.Data);
                Destroy(Card.gameObject);
                return;
            }
        }

        ScenarioManager.inst.MainScheme.Threat.Acceleration++;
    }
}
