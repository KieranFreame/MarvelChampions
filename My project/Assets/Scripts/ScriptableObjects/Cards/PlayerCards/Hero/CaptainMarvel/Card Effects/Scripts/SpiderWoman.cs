using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiderWoman", menuName = "MarvelChampions/Card Effects/Captain Marvel/Spider-Woman")]
public class SpiderWoman : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        var villain = FindObjectOfType<Villain>();
        villain.CharStats.Schemer.Confused = true;

        await Task.Yield();
    }
}
