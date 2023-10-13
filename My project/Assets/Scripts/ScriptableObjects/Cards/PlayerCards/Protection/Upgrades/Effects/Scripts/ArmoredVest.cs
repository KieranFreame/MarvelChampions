using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Armored Vest", menuName = "MarvelChampions/Card Effects/Protection/Upgrades/Armored Vest")]
public class ArmoredVest : PlayerCardEffect
{
    public override Task OnEnterPlay()
    {
        _owner.CharStats.Defender.CurrentDefence++;
        return Task.CompletedTask;
    }

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            //There is not already a Armored Vest in play
            return !_owner.CardsInPlay.Permanents.Any(x => x.CardName == Card.CardName);
        }
        
        return false;
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Defender.CurrentDefence--;
    }
}
