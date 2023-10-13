using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Master", menuName = "MarvelChampions/Card Effects/Modulars/RotRS/Weapon Master/Weapon Master")]
public class WeaponMaster : EncounterCardEffect
{
    public override async Task WhenRevealed(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            await owner.CharStats.InitiateScheme();
        }
        else
        {
            await owner.CharStats.InitiateAttack();
        }

        if (owner.Attachments.Any(x => (x as ICard).CardTraits.Contains("Weapon")))
            ScenarioManager.inst.Surge(player);
    }
}
