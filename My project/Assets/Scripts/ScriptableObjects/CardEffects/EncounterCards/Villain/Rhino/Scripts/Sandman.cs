using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sandman", menuName = "MarvelChampions/Card Effects/Rhino/Sandman")]
public class Sandman : CardEffect
{
    public override void OnEnterPlay(Villain owner, Card card)
    {
        (card as MinionCard).CharStats.Health.Tough = true;
    }
}
