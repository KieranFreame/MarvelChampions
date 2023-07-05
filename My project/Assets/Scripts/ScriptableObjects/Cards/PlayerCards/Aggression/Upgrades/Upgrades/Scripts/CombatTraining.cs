using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Training", menuName = "MarvelChampions/Card Effects/Aggression/Combat Training")]
public class CombatTraining : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack++;
        await Task.Yield();
    }

    public override bool CanBePlayed()
    {
        //There is not already a Combat Training in play
        if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Combat Training"))
            return false;

        return base.CanBePlayed();
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
    }
}
