using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enhanced Spider-Sense", menuName = "MarvelChampions/Card Effects/Spider-Man (Peter Parker)/Enhanced Spider-Sense")]
public class EnhancedSpiderSense : PlayerCardEffect, IOptional
{
    /// <summary>
    /// When a treachery card is revealed, cancel its When Revealed effects
    /// </summary>

    public override void OnDrawn()
    {
        EffectManager.Inst.OnEffectActivated += CanRespond;
    }

    void CanRespond(ICard card)
    {
        if (card.CardType == CardType.Treachery && _owner.ResourcesAvailable(_card) >= 1)
            EffectManager.Inst.Responding.Add(this);
    }

    public override Task OnEnterPlay()
    {
        EffectManager.Inst.OnEffectActivated -= CanRespond;
        EffectManager.Inst.Responding.RemoveAll(x => x.Card.CardName == "Enhanced Spider-Sense");

        return Task.CompletedTask;
    }

    public override Task Resolve()
    {
        Stack<IEffect> stack = new();

        while (EffectManager.Inst.Resolving.Count != 0 && EffectManager.Inst.Resolving.Peek().Card.CardType is not CardType.Treachery)
        {
            stack.Push(EffectManager.Inst.Resolving.Pop());
        }

        EffectManager.Inst.Resolving.Pop(); //Cancel effect

        while (stack.Count > 0)
        {
            EffectManager.Inst.Resolving.Push(stack.Pop());
        }

        return Task.CompletedTask;
    }

    public override void OnDiscard()
    {
        EffectManager.Inst.OnEffectActivated -= CanRespond;
    }
}
