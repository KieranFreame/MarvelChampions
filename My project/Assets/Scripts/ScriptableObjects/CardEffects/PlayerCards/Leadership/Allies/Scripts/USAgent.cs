using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "US Agent", menuName = "MarvelChampions/Card Effects/Leadership/USAgent")]
public class USAgent : PlayerCardEffect
{
    Retaliate _retaliate;

    public override async Task OnEnterPlay(Player owner, PlayerCard card)
    {
        _retaliate = new(card, 1);
        await Task.Yield();
    }
}
