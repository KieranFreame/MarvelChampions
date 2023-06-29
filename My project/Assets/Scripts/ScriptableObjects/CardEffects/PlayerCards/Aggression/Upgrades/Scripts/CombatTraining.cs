using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Training", menuName = "MarvelChampions/Card Effects/Aggression/Combat Training")]
public class CombatTraining : PlayerCardEffect
{
    public override void OnDrawn(Player player, PlayerCard card)
    {
        _owner = player;
        Card = card;
    }

    public override async Task OnEnterPlay(Player player, PlayerCard card)
    {
        _owner.CharStats.Attacker.CurrentAttack++;

        await Task.Yield();
    }

    public override bool CanBePlayed()
    {
        //There is not already a Combat Training in play
        return !_owner.CardsInPlay.Permanents.Any(x => x.CardName == "Combat Training");
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Attacker.CurrentAttack--;
    }
}
