using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "False Alarm", menuName = "MarvelChampions/Card Effects/Bomb Scare/False Alarm")]

public class FalseAlarm : EncounterCardEffect
{
    /// <summary>
    /// When Revealed: You are confused. If you are already confused, Surge
    /// </summary>

    public override Task Resolve()
    {
        var player = TurnManager.instance.CurrPlayer;

        if (player.CharStats.Thwarter.Confused)
            ScenarioManager.inst.Surge(player);
        else
            player.CharStats.Thwarter.Confused = true;

        return Task.CompletedTask;
    }
}
