using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Endurance", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/Endurance")]
public class Endurance : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return !_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Endurance");
        }

        return false;
    }

    public override Task OnEnterPlay()
    {
        _owner.CharStats.Health.IncreaseMaxHealth(3);
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Health.IncreaseMaxHealth(-3);
    }
}
