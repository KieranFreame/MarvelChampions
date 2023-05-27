using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "False Alarm", menuName = "MarvelChampions/Card Effects/Bomb Scare/False Alarm")]

public class FalseAlarm : CardEffect
{
    /// <summary>
    /// When Revealed: You are confused. If you are already confused, Surge
    /// </summary>

    public override void OnEnterPlay(Villain owner, Card card)
    {
        Player player = FindObjectOfType<Player>();

        if (player.CharStats.Thwarter.Confused)
            owner.Surge(player);
        else
            player.CharStats.Thwarter.Confused = true;
    }
}
