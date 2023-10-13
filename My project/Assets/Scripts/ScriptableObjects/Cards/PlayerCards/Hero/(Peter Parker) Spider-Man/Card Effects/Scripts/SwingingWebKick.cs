using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Swinging Web Kick", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Swinging Web Kick")]
public class SwingingWebKick : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        await _owner.CharStats.InitiateAttack(new(8, owner: _owner, card: Card));
    }
}
