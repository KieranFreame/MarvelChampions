using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Uppercut", menuName = "MarvelChampions/Card Effects/Aggression/Uppercut")]
public class Uppercut : PlayerCardEffect
{
    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        await player.CharStats.InitiateAttack(new(5, new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain }, owner: player));
    }
}
