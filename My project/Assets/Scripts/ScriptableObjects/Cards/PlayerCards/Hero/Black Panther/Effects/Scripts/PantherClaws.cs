using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Panther Claws", menuName = "MarvelChampions/Card Effects/Black Panther/Panther Claws")]
public class PantherClaws : PlayerCardEffect, IBlackPanther
{
    public async Task Special (bool last)
    {
        await _owner.CharStats.InitiateAttack(new(last ? 4 : 2, owner:_owner, card: Card));
    }
}
