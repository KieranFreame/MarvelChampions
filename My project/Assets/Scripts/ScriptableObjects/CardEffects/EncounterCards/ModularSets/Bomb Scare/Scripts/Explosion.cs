using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "MarvelChampions/Card Effects/Bomb Scare/Explosion")]
public class Explosion : EncounterCardEffect
{
    /// <summary>
    /// When Revealed: If Bomb Scare is in play, deal X indirect damage where X is the amount of threat on Bomb Scare. If Bomb Scare is not in play, Surge.
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        List<SchemeCard> schemeCards = new();
        schemeCards.AddRange(FindObjectsOfType<SchemeCard>());
        SchemeCard bombScare = schemeCards.FirstOrDefault(x => x.name == "Bomb Scare");

        if (bombScare == null)
        {
            _owner.Surge(FindObjectOfType<Player>());
        }
        else
        {
            List<Health> targets = new()
            {
                FindObjectOfType<Player>().CharStats.Health,
            };

            targets.AddRange(FindObjectOfType<Player>().CardsInPlay.GetHealth());

            IndirectDamageHandler.HandleIndirectDamage(targets, bombScare.GetComponent<Threat>().CurrentThreat);
        }
    }
}
