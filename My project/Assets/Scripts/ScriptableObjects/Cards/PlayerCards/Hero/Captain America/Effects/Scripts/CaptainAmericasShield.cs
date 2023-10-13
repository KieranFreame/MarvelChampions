using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain America's Shield", menuName = "MarvelChampions/Card Effects/Captain America/Captain America's Shield")]
public class CaptainAmericasShield : PlayerCardEffect
{
    Retaliate retaliate;

    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            //Restricted keyword (Cannot control more than 2 Restricted cards)
            return _owner.CardsInPlay.Permanents.Where(x => x.CardTraits.Contains("Restricted")).Count() != 2;
        }

        return false;
    }

    public override Task OnEnterPlay()
    {
        retaliate = new(_owner, 1);
        _owner.CharStats.Defender.CurrentDefence++;

        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        retaliate?.WhenRemoved();
        _owner.CharStats.Defender.CurrentDefence--;
    }
}
