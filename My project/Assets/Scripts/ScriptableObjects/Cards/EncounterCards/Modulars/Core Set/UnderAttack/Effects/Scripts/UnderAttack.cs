using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Under Attack", menuName = "MarvelChampions/Card Effects/Under Attack/Under Attack")]
public class UnderAttack : EncounterCardEffect
{
    Crisis _crisis;

    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        foreach (var p in TurnManager.Players)
        {
            if (p.Identity.ActiveIdentity is Hero)
            {
                int decision = await ChooseEffectUI.ChooseEffect(new List<string>() { "Place 2 threat on Under Attack", "Take 3 damage" });

                if (decision == 2)
                {
                    p.CharStats.Health.TakeDamage(new(p, 2));
                    continue;
                }
            }

            (card as SchemeCard).Threat.GainThreat(2);
        }

        _crisis = new(card as SchemeCard);
    }
}
