using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hydra Mercenary", menuName = "MarvelChampions/Card Effects/Rhino/Hydra Mercenary")]
public class HydraMercenary : EncounterCardEffect
{
    Guard _guard;
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _guard = new(owner, card as MinionCard);
    }
}
