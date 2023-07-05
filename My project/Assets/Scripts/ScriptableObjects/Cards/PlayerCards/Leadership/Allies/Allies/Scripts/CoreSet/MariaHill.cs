using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Maria Hill", menuName = "MarvelChampions/Card Effects/Leadership/Maria Hill")]
public class MariaHill : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        foreach (Player p in TurnManager.Players)
            DrawCardSystem.instance.DrawCards(new(1, p));

        await Task.Yield();
    }
}
