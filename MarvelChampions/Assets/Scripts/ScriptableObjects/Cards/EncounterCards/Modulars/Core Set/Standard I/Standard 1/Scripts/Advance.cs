using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Advance", menuName = "MarvelChampions/Card Effects/Standard I/Advance")]
public class Advance : EncounterCardEffect
{
    public override async Task Resolve() => await _owner.CharStats.InitiateScheme();
}
