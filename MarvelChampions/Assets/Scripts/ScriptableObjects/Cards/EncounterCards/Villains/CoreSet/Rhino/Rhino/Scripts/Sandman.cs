using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sandman", menuName = "MarvelChampions/Card Effects/Rhino/Sandman")]
public class Sandman : EncounterCardEffect
{
    public override Task Resolve()
    {
        ((MinionCard)_card).CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
