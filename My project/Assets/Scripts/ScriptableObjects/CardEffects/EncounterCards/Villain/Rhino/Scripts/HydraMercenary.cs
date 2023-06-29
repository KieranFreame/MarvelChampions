using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Mercenary", menuName = "MarvelChampions/Card Effects/Rhino/Hydra Mercenary")]
public class HydraMercenary : EncounterCardEffect
{
    Guard _guard;
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        _guard = new(owner, card as MinionCard);
        await Task.Yield();
    }
}
