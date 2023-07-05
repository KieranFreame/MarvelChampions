using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Business Problems", menuName = "MarvelChampions/Card Effects/Iron Man/Business Problems")]
public class BusinessProblems : EncounterCardEffect
{
    Player stark;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        stark = TurnManager.Players.FirstOrDefault(x => x.Identity.AlterEgo.Name == "Tony Stark");

        if (stark.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice("Flip to Alter-Ego?");

            if (decision)
            {
                stark.Identity.FlipToAlterEgo();
            }
        }

        if (stark.Identity.ActiveIdentity is AlterEgo && !stark.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Tony Stark. Remove this card from the game", "Exhaust all your upgrades" });

            if (decision == 1)
            {
                stark.Exhaust();
                ScenarioManager.inst.RemoveFromGame(Card.Data);
                Destroy(Card.gameObject);
                return;
            }
        }

        List<PlayerCard> upgrades = stark.CardsInPlay.Permanents.Where(x => x.Data.cardType == CardType.Upgrade).ToList();

        foreach (var c in upgrades)
        {
            c.Exhaust();
        }
    }
}
