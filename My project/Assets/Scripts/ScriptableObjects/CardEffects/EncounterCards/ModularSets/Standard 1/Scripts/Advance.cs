using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Advance", menuName = "MarvelChampions/Card Effects/Standard I/Advance")]
public class Advance : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        _owner.StartCoroutine(_owner.CharStats.InitiateScheme());
    }
}
