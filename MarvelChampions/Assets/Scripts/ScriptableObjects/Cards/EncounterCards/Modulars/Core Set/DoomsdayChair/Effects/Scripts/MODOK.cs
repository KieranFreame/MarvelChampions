using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MODOK", menuName = "MarvelChampions/Card Effects/The Doomsday Chair/MODOK")]
public class MODOK : EncounterCardEffect
{
    Retaliate _retaliate;

    public override Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _retaliate = new(owner, 2);

        return Task.CompletedTask;
    }
}
