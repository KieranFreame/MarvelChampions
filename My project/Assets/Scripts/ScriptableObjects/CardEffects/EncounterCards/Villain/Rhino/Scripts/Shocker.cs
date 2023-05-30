using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shocker", menuName = "MarvelChampions/Card Effects/Rhino/Shocker")]
public class Shocker : EncounterCardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        List<ICharacter> players = new(); 
        players.AddRange(TurnManager.Players);

        card.StartCoroutine(DamageSystem.instance.ApplyDamage(new(players, 1, true)));
    }
}
