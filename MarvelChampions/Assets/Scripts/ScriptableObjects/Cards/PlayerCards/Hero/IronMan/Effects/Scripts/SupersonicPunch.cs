using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Supersonic Punch", menuName = "MarvelChampions/Card Effects/Iron Man/Supersonic Punch")]
public class SupersonicPunch : PlayerCardEffect 
{
    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new(_owner.Identity.IdentityTraits.Contains("Aerial") ? 8 : 4, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, owner: _owner, card: Card));
    }
}
