using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Gang Up", menuName = "MarvelChampions/Card Effects/Standard I/Gang Up")]
public class GangUp : EncounterCardEffect
{
    public override async Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.Identity.ActiveIdentity is not Hero)
        {
            ScenarioManager.inst.Surge(player);
            return;
        }

        await _owner.CharStats.InitiateAttack();

        var minions = VillainTurnController.instance.MinionsInPlay;

        foreach (MinionCard m in minions)
        {
            await m.CharStats.InitiateAttack();
        }
    }
}
