using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "PhotonicBlast", menuName = "MarvelChampions/Card Effects/Captain Marvel/Photonic Blast")]
public class PhotonicBlast : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (base.CanBePlayed())
        {
            return _owner.Identity.ActiveIdentity is Hero;
        }
        
        return false;
    }

    public override async Task OnEnterPlay()
    {
        var action = new AttackAction(5, owner:_owner, card: Card);
        await _owner.CharStats.InitiateAttack(action);

        if (PayCostSystem.instance.Resources.Contains(Resource.Energy))
            DrawCardSystem.Instance.DrawCards(new DrawCardsAction(1));
    }
}
