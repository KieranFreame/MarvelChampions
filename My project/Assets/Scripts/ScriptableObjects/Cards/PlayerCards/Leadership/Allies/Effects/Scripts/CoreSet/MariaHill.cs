using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Maria Hill", menuName = "MarvelChampions/Card Effects/Leadership/Maria Hill")]
public class MariaHill : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        foreach (Player p in TurnManager.Players)
            DrawCardSystem.Instance.DrawCards(new(1, p));

        return Task.CompletedTask;
    }
}
