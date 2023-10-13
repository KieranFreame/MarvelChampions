using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "MarvelChampions/Card Effects/Bomb Scare/Explosion")]
public class Explosion : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        List<SchemeCard> schemeCards = new();
        schemeCards.AddRange(FindObjectsOfType<SchemeCard>());
        SchemeCard bombScare = schemeCards.FirstOrDefault(x => x.name == "Bomb Scare");

        if (bombScare == null)
        {
            ScenarioManager.inst.Surge(player);
        }
        else
        {
            List<ICharacter> targets = new()
            {
                player
            };

            targets.AddRange(player.CardsInPlay.Allies);

            await IndirectDamageHandler.inst.HandleIndirectDamage(targets, bombScare.Threat.CurrentThreat);
        }

        await Task.Yield();
    }
}
