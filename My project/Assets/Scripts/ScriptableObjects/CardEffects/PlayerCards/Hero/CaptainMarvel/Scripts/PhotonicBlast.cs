using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PhotonicBlast", menuName = "MarvelChampions/Card Effects/Captain Marvel/Photonic Blast")]
public class PhotonicBlast : PlayerCardEffect
{
    public override void OnEnterPlay(Player owner, Card card)
    {
        _owner = owner;

        if (_owner.CharStats.Attacker.Stunned)
        {
            _owner.CharStats.Attacker.Stunned = false;
            return;
        }

        var action = new AttackAction(5, new List<TargetType>() { TargetType.TargetMinion, TargetType.TargetVillain}, owner:_owner);
        _owner.StartCoroutine(AttackSystem.instance.InitiateAttack(action));

        if (PayCostSystem.instance.Resources.Contains(Resource.Energy))
            DrawCardSystem.instance.DrawCards(new DrawCardsAction(1));
    }
}
