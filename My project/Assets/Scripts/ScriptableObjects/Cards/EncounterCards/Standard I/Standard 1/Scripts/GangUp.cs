using System.Collections;
using System.Collections.Generic;
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
            owner.Surge(player);
        }
    }

    private async Task HandleAttacks()
    {
        await _owner.CharStats.InitiateAttack();

        foreach (MinionCard m in VillainTurnController.instance.MinionsInPlay)
        {
            await m.CharStats.InitiateAttack();
        }
    }
}
