using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Whiplash", menuName = "MarvelChampions/Card Effects/Nemesis/Iron Man/Whiplash")]
public class Whiplash : EncounterCardEffect
{
    Retaliate _retaliate;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _retaliate = new(card, 1);
        await Task.Yield();
    }
}
