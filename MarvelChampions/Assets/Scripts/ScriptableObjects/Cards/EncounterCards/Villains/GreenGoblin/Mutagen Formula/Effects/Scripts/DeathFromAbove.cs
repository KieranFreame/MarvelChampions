using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Death From Above", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Death From Above")]
public class DeathFromAbove : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is Hero)
        {
            owner.CharStats.Attacker.CurrentAttack += owner.Stages.Stage;
            await owner.CharStats.InitiateAttack();
            owner.CharStats.Attacker.CurrentAttack -= owner.Stages.Stage;
        }
        else
        {
            owner.CharStats.Schemer.CurrentScheme += owner.Stages.Stage;
            await owner.CharStats.InitiateScheme();
            owner.CharStats.Schemer.CurrentScheme -= owner.Stages.Stage;
        }
    }
}
