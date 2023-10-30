using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mark V Armor", menuName = "MarvelChampions/Card Effects/Iron Man/Mark V Armor")]
public class MarkVArmor : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Health.IncreaseMaxHealth(6);
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Health.IncreaseMaxHealth(-6);
    }
}
