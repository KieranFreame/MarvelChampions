using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "US Agent", menuName = "MarvelChampions/Card Effects/Leadership/USAgent")]
public class USAgent : PlayerCardEffect
{
    Retaliate _retaliate;

    public override Task OnEnterPlay()
    {
        _retaliate = new(Card as AllyCard, 1);
        return Task.CompletedTask;
    }
}
