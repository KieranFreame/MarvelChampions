using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Gamma Slam", menuName = "MarvelChampions/Card Effects/She-Hulk/Gamma Slam")]
public class GammaSlam : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return _owner.Identity.ActiveIdentity is Hero;
        }

        return false;
    }

    public override async Task OnEnterPlay()
    {
        Health h = _owner.CharStats.Health;
        int damage = h.BaseHP - h.CurrentHealth;

        if (damage > 15) damage = 15;

        await _owner.CharStats.InitiateAttack(new(damage, owner: _owner, card: Card));
    }
}
