using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "The Sorcerer Supreme", menuName = "MarvelChampions/Card Effects/Basic/Upgrades/The Sorcerer Supreme")]
public class SorcererSupreme : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if(base.CanBePlayed())
        {
            if (!_owner.Identity.IdentityTraits.Contains("Mystic"))
                return false;

            //Unique
            if (_owner.CardsInPlay.Permanents.Any(x => x.CardName == Card.CardName))
                return false;

            return true;
        }

        return false;
    }

    public override Task OnEnterPlay()
    {
        _owner.Identity.Hero.HandSize++;
        return Task.CompletedTask;
    }

    public override void OnExitPlay()
    {
        _owner.Identity.Hero.HandSize--;
    }
}
