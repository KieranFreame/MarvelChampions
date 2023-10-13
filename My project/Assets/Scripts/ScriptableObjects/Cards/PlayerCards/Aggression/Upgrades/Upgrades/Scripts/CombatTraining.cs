using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Training", menuName = "MarvelChampions/Card Effects/Aggression/Combat Training")]
public class CombatTraining : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        return Task.CompletedTask;
    }

    public override bool CanBePlayed()
    {
        if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Combat Training"))
            return false;

        return base.CanBePlayed();
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
    }
}
