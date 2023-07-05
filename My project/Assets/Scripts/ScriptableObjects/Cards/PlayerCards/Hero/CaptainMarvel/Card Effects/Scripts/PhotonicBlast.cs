using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "PhotonicBlast", menuName = "MarvelChampions/Card Effects/Captain Marvel/Photonic Blast")]
public class PhotonicBlast : PlayerCardEffect
{
    public override bool CanBePlayed()
    {
        if (_owner.Identity.ActiveIdentity is not Hero)
            return false;

        return base.CanBePlayed();
    }

    public override async Task OnEnterPlay()
    {
        var action = new AttackAction(5, new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain}, owner:_owner);
        await _owner.CharStats.InitiateAttack(action);

        if (PayCostSystem.instance.Resources.Contains(Resource.Energy))
            DrawCardSystem.instance.DrawCards(new DrawCardsAction(1));
    }
}
