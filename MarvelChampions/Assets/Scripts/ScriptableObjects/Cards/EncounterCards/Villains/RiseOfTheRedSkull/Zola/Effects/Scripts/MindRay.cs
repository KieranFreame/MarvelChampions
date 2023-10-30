using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mind Ray", menuName = "MarvelChampions/Card Effects/RotRS/Zola/Mind Ray")]
public class MindRay : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            player.CharStats.Thwarter.Confused = true;
            await owner.CharStats.InitiateScheme();
        }
        else
        {
            player.CharStats.Attacker.Stunned = true;
            await owner.CharStats.InitiateAttack();
        }
    }
}
