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

    public override async Task OnEnterPlay()
    {
        AttackAction attack;

        if (PayCostSystem.instance.Resources.Contains(Resource.Physical))
            attack = new(5, new List<TargetType>() { TargetType.TargetMinion }, new List<Keywords>() { Keywords.Overkill }, _owner);
        else
            attack = new(5, new List<TargetType>() { TargetType.TargetMinion }, owner:_owner);

        await _owner.CharStats.InitiateAttack(attack);
    }
}
