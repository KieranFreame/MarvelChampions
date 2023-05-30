using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assault", menuName = "MarvelChampions/Card Effects/Standard I/Assault")]
public class Assault : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        _owner = owner;
        var player = FindObjectOfType<Player>();

        if (player.Identity.ActiveIdentity is Hero)
            _owner.StartCoroutine(_owner.CharStats.InitiateAttack());
        else
            owner.Surge(player);
    }
}
