using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "MarvelChampions/Card Effects/Masters of Evil/Whirlwind")]
public class Whirlwind : EncounterCardEffect
{
    public override Task OnEnterPlay()
    {
        Debug.Log("Whirlwind's effect cannot resolve in a solo game");
        return Task.CompletedTask;
    }
}
