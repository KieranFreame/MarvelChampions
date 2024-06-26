using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Home By Dawn", menuName = "MarvelChampions/Card Effects/Ms Marvel/Home By Dawn")]
public class HomeByDawn : EncounterCardEffect
{
    Player khan;
    public override async Task Resolve()
    {
        khan = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Kamala Khan");

        if (khan.Identity.ActiveIdentity is not AlterEgo)
            if (await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?"))
                khan.Identity.FlipToAlterEgo();
        

        if (khan.Identity.ActiveIdentity is AlterEgo && !khan.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Kamala Khan. Remove this card from the game", "Discard a Persona Support. If you cannot, Surge" });

            if (decision == 1)
            {
                khan.Exhaust();
                ScenarioManager.inst.RemoveFromGame(_card.Data);
                Destroy(_card.gameObject);
                return;
            }
        }

        List<PlayerCard> personas = new (khan.CardsInPlay.Permanents.Where(x => x.CardType == CardType.Support && x.CardTraits.Contains("Persona")));
            
        if (personas.Count == 0)
            ScenarioManager.inst.Surge(TurnManager.instance.CurrPlayer);
        else
        {
            PlayerCard discard = await TargetSystem.instance.SelectTarget(personas);
            khan.CardsInPlay.Permanents.Remove(discard);
            khan.Deck.Discard(discard);
        }
    }
}
