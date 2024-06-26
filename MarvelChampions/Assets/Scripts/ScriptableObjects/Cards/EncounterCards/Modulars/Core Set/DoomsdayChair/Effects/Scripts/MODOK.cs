using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MODOK", menuName = "MarvelChampions/Card Effects/The Doomsday Chair/MODOK")]
public class MODOK : EncounterCardEffect
{
    Retaliate _retaliate;

    public override Task OnEnterPlay()
    {
        _retaliate = new(_owner, 2);

        return Task.CompletedTask;
    }
}
