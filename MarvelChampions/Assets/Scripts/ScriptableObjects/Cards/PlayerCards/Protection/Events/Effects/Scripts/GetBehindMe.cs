using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Behind Me", menuName = "MarvelChampions/Card Effects/Protection/Events/Get Behind Me")]
public class GetBehindMe : PlayerCardEffect, IOptional
{
    public override void OnDrawn()
    {
        EffectManager.Inst.OnEffectActivated += CanRespond;
    }

    private void CanRespond(ICard arg0)
    {
        if (arg0.CardType is CardType.Treachery && _owner.ResourcesAvailable(_card) >= _card.CardCost)
            EffectManager.Inst.Responding.Add(this);
    }

    public override Task OnEnterPlay()
    {
        EffectManager.Inst.OnEffectActivated -= CanRespond;
        EffectManager.Inst.Responding.RemoveAll(x => x.Card.CardName == "Get Behind Me");

        return Task.CompletedTask;
    }

    public override async Task Resolve()
    {
        CancelEffect();
        await ScenarioManager.inst.ActiveVillain.CharStats.InitiateAttack();
    }

    public void CancelEffect()
    {
        Stack<IEffect> stack = new Stack<IEffect>();

        while (EffectManager.Inst.Resolving.Peek().Card.CardType is not CardType.Treachery && EffectManager.Inst.Resolving.Count != 0)
        {
            stack.Push(EffectManager.Inst.Resolving.Pop());
        }


        if (EffectManager.Inst.Resolving.Count >= 1)
            EffectManager.Inst.Resolving.Pop(); //Cancel effect

        while (stack.Count > 0)
        {
            EffectManager.Inst.Resolving.Push(stack.Pop());
        }
    }

    public override void OnDiscard()
    {
        EffectManager.Inst.OnEffectActivated -= CanRespond;
    }
}
