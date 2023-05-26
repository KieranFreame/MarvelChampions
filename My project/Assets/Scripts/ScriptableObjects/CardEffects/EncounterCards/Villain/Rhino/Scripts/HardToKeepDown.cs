using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hard To Keep Down", menuName = "MarvelChampions/Card Effects/Rhino/Hard To Keep Down")]
public class HardToKeepDown : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        var health = owner.CharStats.Health;

        if (!health.Damaged())
        {
            owner.Surge(FindObjectOfType<Player>());
            return;
        }

        health.RecoverHealth(4);
    }
}
