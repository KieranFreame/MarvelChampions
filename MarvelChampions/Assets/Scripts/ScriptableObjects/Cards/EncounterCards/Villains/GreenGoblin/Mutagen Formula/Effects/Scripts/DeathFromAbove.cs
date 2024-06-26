using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Death From Above", menuName = "MarvelChampions/Card Effects/Mutagen Formula/Death From Above")]
public class DeathFromAbove : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.Identity.ActiveIdentity is Hero)
        {
            _owner.CharStats.Attacker.CurrentAttack += _owner.Stages.Stage;
            await _owner.CharStats.InitiateAttack();
            _owner.CharStats.Attacker.CurrentAttack -= _owner.Stages.Stage;
        }
        else
        {
            _owner.CharStats.Schemer.CurrentScheme += _owner.Stages.Stage;
            await _owner.CharStats.InitiateScheme();
            _owner.CharStats.Schemer.CurrentScheme -= _owner.Stages.Stage;
        }
    }
}
