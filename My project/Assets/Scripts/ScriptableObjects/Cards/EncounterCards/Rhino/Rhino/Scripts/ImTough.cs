using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "I'm Tough", menuName = "MarvelChampions/Card Effects/Rhino/Im Tough")]
public class ImTough : EncounterCardEffect
{
    /// <summary>
    /// Give Rhino a tough status card. If Rhino already has a tough status card, this card gains surge.
    /// </summary>
    public override async Task OnEnterPlay(Villain owner, EncounterCard card, Player player)
    {
        if (owner.CharStats.Health.Tough)
        {
            owner.Surge(player);
            return;
        }

        owner.CharStats.Health.Tough = true;

        await Task.Yield();
    }
}
