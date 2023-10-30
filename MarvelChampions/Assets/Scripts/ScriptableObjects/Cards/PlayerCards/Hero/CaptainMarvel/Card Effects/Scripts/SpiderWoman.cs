using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiderWoman", menuName = "MarvelChampions/Card Effects/Captain Marvel/Spider-Woman")]
public class SpiderWoman : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        var villain = ScenarioManager.inst.ActiveVillain;
        villain.CharStats.Schemer.Confused = true;

        return Task.CompletedTask;
    }
}
