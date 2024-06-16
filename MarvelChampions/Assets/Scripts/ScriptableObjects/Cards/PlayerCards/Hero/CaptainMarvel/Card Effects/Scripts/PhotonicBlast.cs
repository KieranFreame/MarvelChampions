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
        var action = new AttackAction(5, targets: new() { TargetType.TargetVillain, TargetType.TargetMinion }, attackType: AttackType.Card, owner:_owner, card: Card);

        if (await _owner.CharStats.InitiateAttack(action) && PayCostSystem.instance.Resources.Contains(Resource.Energy)) //if the attack goes through
            DrawCardSystem.Instance.DrawCards(new DrawCardsAction(1));
    }
}
