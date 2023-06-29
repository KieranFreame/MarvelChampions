using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(fileName = "Family Emergency", menuName = "MarvelChampions/Card Effects/Captain Marvel/Family Emergency")]
public class FamilyEmergency : EncounterCardEffect
{
    Player danvers;
    Player currPlayer;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        Card = card;
        currPlayer = player;
        danvers = TurnManager.Players.FirstOrDefault(x => x.Identity.Hero.Name == "Captain Marvel");

        if (danvers.Identity.ActiveIdentity is not AlterEgo)
        {
            bool decision = await ConfirmActivateUI.MakeChoice(Card, "Flip to Alter-Ego?");

            if (decision)
            {
                danvers.Identity.FlipToAlterEgo();
            }
        }

        if (danvers.Identity.ActiveIdentity is AlterEgo && !danvers.Exhausted) //chose to flip\was already alterego && is not exhausted
        {
            int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Exhaust Carol Danvers. Remove Family Emergency from the game", "Become stunned. Surge." });

            switch (decision)
            {
                case 1:
                    danvers.Exhaust();
                    ScenarioManager.inst.RemoveFromGame(Card.Data);
                    Destroy(Card.gameObject);
                    break;
                case 2:
                    danvers.CharStats.Attacker.Stunned = true;
                    _owner.Surge(currPlayer);
                    break;
            }
        }
        else
        {
            danvers.CharStats.Attacker.Stunned = true;
            owner.Surge(player);
        }
    }
}
