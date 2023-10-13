using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Tactical Genius", menuName = "MarvelChampions/Card Effects/Black Panther/Tactical Genius")]
public class TacticalGenius : PlayerCardEffect, IBlackPanther
{
    public async Task Special(bool last)
    {
        await _owner.CharStats.InitiateThwart(new(last ? 2 : 1));
    }
}
