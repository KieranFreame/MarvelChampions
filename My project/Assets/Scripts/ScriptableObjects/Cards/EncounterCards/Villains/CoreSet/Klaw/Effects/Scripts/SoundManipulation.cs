using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Manipulation", menuName = "MarvelChampions/Card Effects/Klaw/Sound Manipulation")]
public class SoundManipulation : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            if (!owner.CharStats.Health.Damaged())
            {
                ScenarioManager.inst.Surge(player);
                return;
            }

            owner.CharStats.Health.RecoverHealth(4);
            await Task.Yield();
        }
        else //hero
        {
            player.CharStats.Health.TakeDamage(new(player, 2, card:card, owner:owner));
            owner.CharStats.Health.RecoverHealth(2);
        }
    }
}
