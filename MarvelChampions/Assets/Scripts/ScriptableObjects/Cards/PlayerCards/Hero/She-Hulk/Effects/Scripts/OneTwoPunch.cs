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
        GameStateManager.Instance.OnActivationCompleted += IsTriggerMet;
    }

    public override bool CanBePlayed()
    {
        return false;
    }

    private void IsTriggerMet(Action action)
    {
        if (action is not AttackAction || action.Owner.Name != "She-Hulk") return;

        var attack = (AttackAction)action;

        if (attack.AttackType == AttackType.Basic && _owner.ResourcesAvailable(_card) >= _card.CardCost)
            EffectManager.Inst.Responding.Add(this);
    }

    public override Task Resolve()
    {
        _owner.Ready();
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
        _owner.Deck.Discard(_card);

        return Task.CompletedTask;
    }

    public override void OnDiscard()
    {
        GameStateManager.Instance.OnActivationCompleted -= IsTriggerMet;
    }
}
