using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Advance", menuName = "MarvelChampions/Card Effects/Standard I/Advance")]
public class Advance : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _owner = owner;
        await _owner.CharStats.InitiateScheme();
    }
}
