using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "US Agent", menuName = "MarvelChampions/Card Effects/Leadership/USAgent")]
public class USAgent : PlayerCardEffect
{
    Retaliate _retaliate;

    public override void OnEnterPlay(Player owner, Card card)
    {
        _retaliate = new(card, 1);
    }
}
