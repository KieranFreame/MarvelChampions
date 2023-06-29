using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Sandman", menuName = "MarvelChampions/Card Effects/Rhino/Sandman")]
public class Sandman : EncounterCardEffect
{
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        (card as MinionCard).CharStats.Health.Tough = true;
        await Task.Yield();
    }
}
