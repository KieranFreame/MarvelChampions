using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Supersonic Punch", menuName = "MarvelChampions/Card Effects/Iron Man/Supersonic Punch")]
public class SupersonicPunch : PlayerCardEffect 
{
    public override async Task OnEnterPlay()
    {
        if (_owner.Identity.IdentityTraits.Contains("Aerial"))
            await _owner.CharStats.InitiateAttack(new(8, new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner));
        else
            await _owner.CharStats.InitiateAttack(new(4, new List<TargetType>() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner));
    }
}
