using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Manipulation", menuName = "MarvelChampions/Card Effects/Klaw/Sound Manipulation")]
public class SoundManipulation : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.Identity.ActiveIdentity is AlterEgo)
        {
            if (!_owner.CharStats.Health.Damaged())
            {
                ScenarioManager.inst.Surge(player);
                return;
            }

            _owner.CharStats.Health.CurrentHealth += 4;
            await Task.Yield();
        }
        else //hero
        {
            player.CharStats.Health.TakeDamage(new(player, 2, card:_card, owner:_owner));
            _owner.CharStats.Health.CurrentHealth += 2;
        }
    }
}
