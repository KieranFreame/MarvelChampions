using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Affairs of State", menuName = "MarvelChampions/Card Effects/Black Panther/Affairs of State")]
public class AffairsState : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var tchalla = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "T'Challa");

        if (tchalla.Identity.ActiveIdentity is not AlterEgo)
        {
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
            {
                tchalla.Identity.FlipToAlterEgo();
            }
        }

        List<PlayerCard> upgrades = new(tchalla.CardsInPlay.Permanents.Where(x => x.CardTraits.Contains("Black Panther")));
        
        if ((tchalla.Identity.ActiveIdentity is Hero || tchalla.Exhausted) && upgrades.Count == 0)
        {
            Debug.Log("The effect of Affairs of State cannot be resolved. This card gains Surge");
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
            return;
        }

        if (tchalla.Identity.ActiveIdentity is AlterEgo && !tchalla.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            if (upgrades.Count == 0)
            {
                tchalla.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }

            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust T'Challa. Remove this card from the game", "Discard a Black Panther upgrade you control" });

            if (decision == 1)
            {
                tchalla.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        Debug.Log("Discard a Black Panther Upgrade you control");
        PlayerCard p = await TargetSystem.instance.SelectTarget(upgrades);

        tchalla.CardsInPlay.Permanents.Remove(p);
        tchalla.Deck.Discard(p);
    }
}
