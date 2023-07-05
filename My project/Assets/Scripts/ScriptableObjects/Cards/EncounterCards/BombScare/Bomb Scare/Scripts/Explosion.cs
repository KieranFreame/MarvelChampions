using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "MarvelChampions/Card Effects/Bomb Scare/Explosion")]
public class Explosion : EncounterCardEffect
{
    /// <summary>
    /// When Revealed: If Bomb Scare is in play, deal X indirect damage where X is the amount of threat on Bomb Scare. If Bomb Scare is not in play, Surge.
    /// </summary>

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        List<SchemeCard> schemeCards = new();
        schemeCards.AddRange(FindObjectsOfType<SchemeCard>());
        SchemeCard bombScare = schemeCards.FirstOrDefault(x => x.name == "Bomb Scare");

        if (bombScare == null)
        {
            _owner.Surge(player);
        }
        else
        {
            List<ICharacter> targets = new()
            {
                player
            };

            targets.AddRange(player.CardsInPlay.Allies);

            IndirectDamageHandler.HandleIndirectDamage(targets, bombScare.GetComponent<Threat>().CurrentThreat);
        }

        await Task.Yield();
    }
}
