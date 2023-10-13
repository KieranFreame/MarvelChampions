using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Armored Guard", menuName = "MarvelChampions/Card Effects/Klaw/Armored Guard")]
public class ArmoredGuard : EncounterCardEffect
{
    Guard guard;

    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        guard = new(card as MinionCard);
        (card as MinionCard).CharStats.Health.Tough = true;

        await Task.Yield();
    }
}
