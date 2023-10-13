using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Gang Up", menuName = "MarvelChampions/Card Effects/Standard I/Gang Up")]
public class GangUp : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        Card = card;
        _owner = owner;

        if (player.Identity.ActiveIdentity is Hero)
        {
            await HandleAttacks();
        }
        else
        {
            ScenarioManager.inst.Surge(player);
        }
    }

    private async Task HandleAttacks()
    {
        await _owner.CharStats.InitiateAttack();

        List<MinionCard> minions = VillainTurnController.instance.MinionsInPlay.ToList();

        foreach (MinionCard m in minions)
        {
            await m.CharStats.InitiateAttack();
        }
    }
}
