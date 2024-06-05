using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "One-Two Punch", menuName = "MarvelChampions/Card Effects/She-Hulk/One-Two Punch")]
public class OneTwoPunch : PlayerCardEffect, IOptional
{
    public override void OnDrawn()
    {
        AttackSystem.Instance.OnAttackCompleted.Add(IsTriggerMet);
    }

    public override bool CanBePlayed()
    {
        return false;
    }

    private void IsTriggerMet(AttackAction action)
    {
        if (action.Keywords.Contains("Basic"))
            if ((action.Owner as Player).Identity.IdentityName == "She-Hulk")
                EffectResolutionManager.Instance.ResolvingEffects.Push(this);
    }

    public override bool CanActivate()
    {
        return _owner.ResourcesAvailable(_card) < _card.CardCost;
    }

    public override async Task Resolve()
    {
        await PlayCardSystem.Instance.InitiatePlayCard(new(_card));

        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
        _owner.Ready();

        _owner.Deck.Discard(_card);
    }

    public override void OnDiscard()
    {
        AttackSystem.Instance.OnAttackCompleted.Remove(IsTriggerMet);
    }
}
