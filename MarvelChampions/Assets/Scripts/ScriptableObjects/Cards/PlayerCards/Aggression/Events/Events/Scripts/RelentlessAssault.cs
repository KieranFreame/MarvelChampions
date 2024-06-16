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
        AttackAction attack = new(5, new List<TargetType>() { TargetType.TargetMinion }, AttackType.Card, owner: _owner) ;

        if (PayCostSystem.instance.Resources.Contains(Resource.Physical))
            attack.Keywords.Add(Keywords.Overkill);

        await _owner.CharStats.InitiateAttack(attack);
    }
}
