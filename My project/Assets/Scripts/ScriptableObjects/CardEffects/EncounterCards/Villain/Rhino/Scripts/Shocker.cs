using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shocker", menuName = "MarvelChampions/Card Effects/Rhino/Shocker")]
public class Shocker : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        List<Health> players = new();

        foreach (Player p in TurnManager.Players)
            players.Add(p.Identity.CharStats.Health);

        _card.StartCoroutine(DamageSystem.ApplyDamage(new DamageAction(players, 1, false)));
    }
}