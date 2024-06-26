using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Melter's attacks must be defended by an ally, if possible.
/// </summary>

[CreateAssetMenu(fileName = "Melter (Boost)", menuName = "MarvelChampions/Card Effects/Masters of Evil/Melter (Boost)")]
public class MelterBoost : EncounterCardEffect
{
    public override Task Resolve()
    {
        foreach (var a in TurnManager.instance.CurrPlayer.CardsInPlay.Allies)
            a.Exhaust();

        return Task.CompletedTask;
    }
}
