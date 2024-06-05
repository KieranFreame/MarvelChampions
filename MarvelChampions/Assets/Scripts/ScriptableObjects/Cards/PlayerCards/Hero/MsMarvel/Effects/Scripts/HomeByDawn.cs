using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Home By Dawn", menuName = "MarvelChampions/Card Effects/Ms Marvel/Home By Dawn")]
public class HomeByDawn : EncounterCardEffect
{
    Player khan;
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        khan = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Kamala Khan");

        if (khan.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?");

            if (decision)
            {
                khan.Identity.FlipToAlterEgo();
            }
        }

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

        List<PlayerCard> personas = khan.CardsInPlay.Permanents.Where(x => x.CardType == CardType.Support && x.CardTraits.Contains("Persona")).ToList();
            
        if (personas.Count == 0)
            ScenarioManager.inst.Surge(player);
        else
        {
            PlayerCard discard = await TargetSystem.instance.SelectTarget(personas);
            khan.CardsInPlay.Permanents.Remove(discard);
            khan.Deck.Discard(discard);
        }
    }
}
