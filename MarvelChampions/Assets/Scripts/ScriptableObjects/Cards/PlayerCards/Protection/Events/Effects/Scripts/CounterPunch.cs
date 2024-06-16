using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter-Punch", menuName = "MarvelChampions/Card Effects/Protection/Events/Counter-Punch")]
public class CounterPunch : PlayerCardEffect, IOptional
{
    /// <summary>
    /// After your hero defends against an attack, deal damage to that enemy equal to your hero's ATK
    /// </summary>

    ICharacter target;

    public override void OnDrawn()
    {
       GameStateManager.Instance.OnActivationCompleted += CanRespond;
    }

    private void CanRespond(Action action)
    {
        if (action is not AttackAction || ((AttackAction)action).Target != Owner || DefendSystem.Instance.Target != Owner) 
            return;

        target = action.Owner;
        EffectManager.Inst.Responding.Add(this);
    }

    public override Task OnEnterPlay()
    {
        GameStateManager.Instance.OnActivationCompleted -= CanRespond;
        return Task.CompletedTask;
    }

    public override async Task Resolve()
    {
        await AttackSystem.Instance.InitiateAttack(new(Owner.CharStats.Attacker.CurrentAttack, target, AttackType.Card, owner:Owner, card:_card));
    }

    public override void OnDiscard()
    {
        GameStateManager.Instance.OnActivationCompleted -= CanRespond;
    }
}
