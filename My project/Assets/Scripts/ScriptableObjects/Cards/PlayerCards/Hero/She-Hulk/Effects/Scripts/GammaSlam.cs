using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Gamma Slam", menuName = "MarvelChampions/Card Effects/She-Hulk/Gamma Slam")]
public class GammaSlam : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;

        return true;
    }

    public override async Task OnEnterPlay()
    {
        Health h = _owner.CharStats.Health;
        int damage = h.BaseHP - h.CurrentHealth;

        if (damage > 15) damage = 15;

        await _owner.CharStats.InitiateAttack(new(damage, new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner));
    }
}
