using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Assault", menuName = "MarvelChampions/Card Effects/Standard I/Assault")]
public class Assault : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;

        if (player.Identity.ActiveIdentity is Hero)
            await _owner.CharStats.InitiateAttack();
        else
            ScenarioManager.inst.Surge(player);
    }
}
