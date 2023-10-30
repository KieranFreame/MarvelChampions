using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Affairs of State", menuName = "MarvelChampions/Card Effects/Black Panther/Affairs of State")]
public class AffairsState : EncounterCardEffect
{
    Player tchalla;
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        tchalla = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "T'Challa");

        if (tchalla.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?");

            if (decision)
            {
                tchalla.Identity.FlipToAlterEgo();
            }
        }

        List<PlayerCard> upgrades = tchalla.CardsInPlay.Permanents.Where(x => x.CardTraits.Contains("Black Panther")).ToList();
        
        if ((tchalla.Identity.ActiveIdentity is Hero || tchalla.Exhausted) && upgrades.Count == 0)
        {
            Debug.Log("The effect of Affairs of State cannot be resolved. This card gains Surge");
            ScenarioManager.inst.Surge(player);
            return;
        }

        if (tchalla.Identity.ActiveIdentity is AlterEgo && !tchalla.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            if (upgrades.Count == 0)
            {
                tchalla.Exhaust();
                ScenarioManager.inst.RemoveFromGame(Card.Data);
                Destroy(Card.gameObject);
                return;
            }

            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust T'Challa. Remove this card from the game", "Discard a Black Panther upgrade you control" });

            if (decision == 1)
            {
                tchalla.Exhaust();
                ScenarioManager.inst.RemoveFromGame(Card.Data);
                Destroy(Card.gameObject);
                return;
            }
        }

        Debug.Log("Discard an Upgrade or Support you control");
        PlayerCard p = await TargetSystem.instance.SelectTarget(upgrades);

        tchalla.CardsInPlay.Permanents.Remove(p);
        tchalla.Deck.Discard(p);
    }
}
