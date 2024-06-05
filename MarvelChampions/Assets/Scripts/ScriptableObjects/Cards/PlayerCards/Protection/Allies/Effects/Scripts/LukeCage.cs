using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Luke Cage", menuName = "MarvelChampions/Card Effects/Protection/Allies/Luke Cage")]
public class LukeCage : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        (Card as AllyCard).CharStats.Health.Tough = true;
        return Task.CompletedTask;
    }
}
