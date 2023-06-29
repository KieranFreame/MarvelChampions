using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Relentless Assault", menuName = "MarvelChampions/Card Effects/Aggression/Relentless Assault")]
public class RelentlessAssault : PlayerCardEffect
{
    public override bool CanActivate()
    {
        return VillainTurnController.instance.MinionsInPlay.Count > 0;
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        AttackAction attack;

        if (PayCostSystem.instance.Resources.Contains(Resource.Physical))
            attack = new(5, new List<TargetType>() { TargetType.TargetMinion }, new List<Keywords>() { Keywords.Overkill }, player);
        else
            attack = new(5, new List<TargetType>() { TargetType.TargetMinion }, owner:player);

        await player.CharStats.InitiateAttack(attack);
    }
}
