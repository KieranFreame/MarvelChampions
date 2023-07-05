using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Armored Vest", menuName = "MarvelChampions/Card Effects/Protection/Armored Vest")]
public class ArmoredVest : PlayerCardEffect
{
    public override async Task OnEnterPlay()
    {
        _owner.CharStats.Defender.CurrentDefence++;
        await Task.Yield();
    }

    public override bool CanBePlayed()
    {
        //There is not already a Armored Vest in play
        return !_owner.CardsInPlay.Permanents.Any(x => x.CardName == Card.CardName);
    }

    public override void OnExitPlay()
    {
        _owner.CharStats.Defender.CurrentDefence--;
    }
}
