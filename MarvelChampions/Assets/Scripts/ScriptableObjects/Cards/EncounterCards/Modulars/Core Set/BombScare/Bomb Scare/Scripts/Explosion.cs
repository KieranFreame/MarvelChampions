using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "MarvelChampions/Card Effects/Bomb Scare/Explosion")]
public class Explosion : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;
        SchemeCard bombScare = ScenarioManager.sideSchemes.FirstOrDefault(x => x.name == "Bomb Scare");

        if (bombScare == default)
        {
            ScenarioManager.inst.Surge(player);
        }
        else
        {
            List<ICharacter> targets = new(player.CardsInPlay.Allies)
            {
                player
            };

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, bombScare.Threat.CurrentThreat);
        }

        return;
    }
}
