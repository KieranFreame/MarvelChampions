using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussion Grenade", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Weapon Master/Concussion Grenade")]
public class ConcussionGrenade : EncounterCardEffect
{
    public override Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            if (player.CharStats.Confusable.Confused)
            {
                ScenarioManager.inst.MainScheme.Threat.GainThreat(2);
            }
            else
            {
                player.CharStats.Confusable.Confused = true;
                ScenarioManager.inst.MainScheme.Threat.GainThreat(1);
            }
        }
        else
        {
            if (player.CharStats.Attacker.Stunned)
            {
                player.CharStats.Health.TakeDamage(new(player, 2));
            }
            else
            {
                player.CharStats.Attacker.Stunned = true;
                player.CharStats.Health.TakeDamage(new(player, 1));
            }
        }

        return Task.CompletedTask;
    }
}
